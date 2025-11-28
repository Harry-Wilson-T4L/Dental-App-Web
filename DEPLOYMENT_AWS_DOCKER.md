## DentalDrill.CRM – AWS Deployment with Docker

This guide documents the end-to-end process for packaging the DentalDrill.CRM application into a Docker image and deploying it on AWS (ECR + ECS/Fargate). It assumes the codebase is already building correctly (see `BUILD_FIXES.md`) and focuses exclusively on containerization and cloud deployment.

---

### 1. Prerequisites

| Requirement | Details |
|-------------|---------|
| AWS Account | Permissions to create ECR repositories, ECS task definitions/services, IAM roles, and (optionally) EFS. |
| Workstation tools | `git`, `dotnet 6 SDK`, `node 18+`, `npm`, `docker`, `aws` CLI v2. |
| AWS CLI profile | Run `aws configure` and provide IAM credentials that can push to ECR and manage ECS. |
| Docker context | Linux/WSL recommended (Dockerfile uses apt-get). |
| TLS/Secrets | Connection strings, SMTP creds, etc. should **not** be baked into images. These will be injected via AWS SSM Parameter Store or ECS task definition environment variables. |

---

### 2. Build & Publish the Application

1. **Restore npm & .NET deps**
   ```powershell
   cd src
   npm install
   cd DentalDrill.CRM
   npm install
   dotnet restore
   ```

2. **Publish release build**
   ```powershell
   dotnet publish DentalDrill.CRM.csproj `
     -c Release `
     -o ..\..\artifacts\publish\DentalDrill.CRM
   ```
   Output folder (referred to below as `$PUBLISH_DIR`) should contain `DentalDrill.CRM.dll` and all runtime assets.

---

### 3. Prepare the Docker Build Context

The Dockerfile under `src/build/scripts/docker/` expects the following structure:

```
Container/
  app/
    (published output)
    docker-entrypoint.sh
  Dockerfile
```

Two supported workflows:

#### Option A – Use `Prepare-Container.ps1` (preferred in CI)

```powershell
cd src/build/scripts/docker
.\Prepare-Container.ps1
```

This script:

- Extracts the published zip (`drop/DentalDrill.CRM.zip`) into `Container/app`.
- Copies `docker-entrypoint.sh` and `Dockerfile`.
- Removes dev-only config files (tsconfig, package-lock, etc.).
- Removes directories that should be provided as volumes (`wwwroot/images`, `wwwroot/files`).

#### Option B – Manual copy (local dev)

```powershell
$containerRoot = "$PWD\Container"
New-Item -ItemType Directory -Force -Path "$containerRoot\app" | Out-Null
Copy-Item ..\..\artifacts\publish\DentalDrill.CRM\* "$containerRoot\app" -Recurse
Copy-Item docker-entrypoint.sh "$containerRoot\app\docker-entrypoint.sh"
Copy-Item Dockerfile "$containerRoot\Dockerfile"
Remove-Item "$containerRoot\app\appsettings*.json" -Force
```

**Important volumes:** the Dockerfile declares

- `/app/wwwroot/images`
- `/app/wwwroot/files`
- `/app/data-protection`

Mount persistent AWS EFS/EBS volumes or S3-mounted filesystems to preserve uploads and ProtectData keys.

---

### 4. Build & Tag the Docker Image

```powershell
cd Container
docker build -t dentaldrill-crm:latest .
```

For reproducible builds tag with semantic/app version, e.g.

```powershell
$version = "1.0.0"
docker tag dentaldrill-crm:latest 123456789012.dkr.ecr.ap-southeast-2.amazonaws.com/dentaldrill-crm:$version
docker tag dentaldrill-crm:latest 123456789012.dkr.ecr.ap-southeast-2.amazonaws.com/dentaldrill-crm:latest
```

---

### 5. Push Image to AWS ECR

1. **Create repo (one-time)**
   ```bash
   aws ecr create-repository \
     --repository-name dentaldrill-crm \
     --image-scanning-configuration scanOnPush=true \
     --region ap-southeast-2
   ```

2. **Authenticate docker to ECR**
   ```bash
   aws ecr get-login-password --region ap-southeast-2 | \
   docker login --username AWS --password-stdin 123456789012.dkr.ecr.ap-southeast-2.amazonaws.com
   ```

3. **Push tags**
   ```bash
   docker push 123456789012.dkr.ecr.ap-southeast-2.amazonaws.com/dentaldrill-crm:$version
   docker push 123456789012.dkr.ecr.ap-southeast-2.amazonaws.com/dentaldrill-crm:latest
   ```

---

### 6. Configure AWS Infrastructure (ECS Fargate Example)

1. **Networking**
   - Create VPC, private subnets, and security groups allowing HTTPS/HTTP traffic.
   - Provision an Application Load Balancer (ALB) if exposing publicly.

2. **Persistent Storage**
   - Use EFS/File-system for `/app/wwwroot/images`, `/app/wwwroot/files`, `/app/data-protection`.
   - Create access points and mount targets in the same subnets as ECS tasks.

3. **Secrets & Config**
   - Store connection strings, SMTP creds, API keys in AWS Secrets Manager or SSM Parameter Store.
   - Example env vars:
     - `ASPNETCORE_ENVIRONMENT=Production`
     - `ASPNETCORE_URLS=http://+:80`
     - `ASPNETCORE_APPSETTINGS` (optional JSON payload written to `appsettings.json` by `docker-entrypoint.sh`)
     - `ConnectionStrings__DefaultConnection`
     - `Email__AmazonSES__AccessKey` etc.

4. **ECS Task Definition**
   - Launch type: Fargate
   - CPU/Memory: e.g., 1 vCPU / 2 GB
   - Container definition:
     - Image: `123456789012.dkr.ecr.ap-southeast-2.amazonaws.com/dentaldrill-crm:$version`
     - Port mappings: `80/tcp`
     - Environment + secrets referencing SSM parameters
     - Mount EFS volumes to `/app/wwwroot/images`, `/app/wwwroot/files`, `/app/data-protection`
     - Log configuration: AWS Logs (group per environment)

5. **ECS Service**
   - Associate task definition
   - Desired count >= 2 for HA
   - Attach to ALB target group
   - Enable health checks (path `/health` if implemented, otherwise `/`)
   - Auto-scaling policies (CPU/Memory)

---

### 7. Running Locally (Optional Smoke Test)

```bash
docker run --rm -p 8080:80 \
  -e ASPNETCORE_ENVIRONMENT=Development \
  -e ASPNETCORE_APPSETTINGS="$(Get-Content ./appsettings.Development.json -Raw)" \
  -v $(pwd)/local-images:/app/wwwroot/images \
  dentaldrill-crm:latest
```

Open http://localhost:8080 to verify.

---

### 8. Deployment Pipeline Suggestions

1. **CI Build (Azure Pipelines/GitHub Actions)**
   - Reuse steps from `pipeline/DentalDrill.CRM-Build.yml`
   - After `dotnet publish`, run `src/build/scripts/docker/Prepare-Container.ps1`
   - Build Docker image and push to ECR

2. **CD (AWS CodePipeline / GitHub Actions)**
   - Trigger on new ECR image tag
   - Update ECS service via `aws ecs update-service --force-new-deployment ...`

3. **Versioning**
   - Tag images with both semantic version (`1.3.0`) and commit SHA for traceability
   - Include build metadata in `appsettings.BuildInfo.json` (auto-generated by `Prepare-Container.ps1`)

---

### 9. Troubleshooting

| Symptom | Fix |
|---------|-----|
| ECS task stuck in `PENDING` | Ensure subnets have ENI capacity; verify execution role has `AmazonECSTaskExecutionRolePolicy`. |
| Container exits immediately | Check CloudWatch logs; confirm `ASPNETCORE_APPSETTINGS` is valid JSON and includes DB connection strings. |
| Missing uploads after restart | Confirm volumes (EFS/EBS) are mounted to `/app/wwwroot/images` and `/app/wwwroot/files`. |
| `dotnet` not found inside container | Ensure published output copied to `Container/app` before `docker build`. |
| 403/HTTPS issues | Configure ALB listener rules + ACM certificates; container itself only needs HTTP (ALB handles TLS). |

---

### 10. Reference Paths

- Docker artifacts: `src/build/scripts/docker`
- Build pipeline sample: `pipeline/DentalDrill.CRM-Build.yml`
- Published artifacts (local): `artifacts/publish/DentalDrill.CRM`
- Entry point script: `src/build/scripts/docker/docker-entrypoint.sh`

---

By following this guide you can consistently package DentalDrill.CRM into a hardened container image, publish it to AWS ECR, and run it on ECS/Fargate (or EKS/EC2 with minimal changes). Adjust resource sizing, auto-scaling, and secrets management according to your production needs.

