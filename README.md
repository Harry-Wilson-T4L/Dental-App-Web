# Dental Drill Solutions CRM

## Getting started

### Required tools

- .NET SDK 6: https://dotnet.microsoft.com/en-us/download/dotnet/6.0
- NodeJS (18 works, higher version should work too but not tested): https://nodejs.org/en/download/prebuilt-installer
- Microsoft SQL Server 2019 (or newer) Developer Edition: https://go.microsoft.com/fwlink/?linkid=866662

> **Disclaimer**
This product is built using [Telerik & Kendo UI](https://www.telerik.com/). License terms require any developer working & deploying the software using Telerik components to have an assigned license. 

### Recommended tools

- Visual Studio 2022 (latest Preview channel works best with TypeScript tooling): https://visualstudio.microsoft.com/

## Building with Visual Studio

> **📖 Troubleshooting:** If you encounter build errors, see [BUILD_FIXES.md](BUILD_FIXES.md) for detailed solutions and step-by-step fix instructions.

### Quick Setup (Recommended for New Developers)

**After cloning the repository, simply double-click `setup.bat` in the root directory.**

This automated script will:
- ✓ Check prerequisites (.NET SDK, Node.js, npm)
- ✓ Install all npm packages (Kendo UI, TypeScript, and dependencies)
- ✓ Restore all NuGet packages
- ✓ Build the solution to verify everything works

After the script completes, you can open the solution in Visual Studio and build without errors.

### Manual Setup (Alternative)

If you prefer to set up manually or the automated script doesn't work:

1. **Install prerequisites**
   - .NET 6 SDK (`winget install Microsoft.DotNet.SDK.6`)
   - Node.js 18 LTS (`winget install OpenJS.NodeJS.LTS`)
   - Telerik/Kendo UI developer license (required to restore commercial packages)
   - SQL Server Developer edition and SQL Server Management Studio
2. **Clone and open the solution**
   - `git clone <repo>`
   - Open `src/DentalDrill.CRM.sln` in Visual Studio (Solution Explorer view)
3. **Restore .NET dependencies**
   - `Build > Restore NuGet Packages`
4. **Restore TypeScript/Kendo assets (two supported options)**
   - _Task Runner / NPM_: Right-click `package.json` inside the `DentalDrill.CRM` project → `Restore Packages`
   - _Command line_: `cd src/DentalDrill.CRM && npm install`
5. **Verify TypeScript tooling**
   - Visual Studio picks up `tsconfig.json` automatically (`compileOnSave`, ES5 target, explicit Kendo typings)
   - You can also compile manually with `npx tsc --pretty`
6. **Build & run**
   - Set `DentalDrill.CRM` as the startup project
   - Choose `IIS Express` or `DentalDrill.CRM` profile
   - Hit `F5` to launch

### TypeScript & Kendo dependencies

The web project (`src/DentalDrill.CRM/package.json`) already includes the required toolchain:

- Runtime: `@progress/kendo-ui`, `jquery`, `jquery-ui`, `bootstrap`
- Type definitions: `@types/kendo-ui`, `@types/jquery`, `@types/jqueryui`, `@types/bootstrap`
- Tooling: `typescript@^4.9`, `gulp` pipeline (sass/minification), `sass`

After running `npm install`, Visual Studio’s TypeScript build (via `Microsoft.TypeScript.MSBuild`) will transpile files tracked under `wwwroot/js/app/**/*.ts`. Kendo UI widgets can be imported via the global `kendo` namespace once the npm packages are restored.

### Command-line validation

To mirror the Visual Studio build, you can run:

```powershell
cd src
dotnet build DentalDrill.CRM.sln
```

If the build succeeds here, Visual Studio will also succeed (same MSBuild pipeline). Address any warnings reported during the build (e.g., outdated target framework or vulnerable NuGet packages) before promoting to higher environments.

## Deployment

- **AWS (Docker/ECS):** see [DEPLOYMENT_AWS_DOCKER.md](DEPLOYMENT_AWS_DOCKER.md) for step-by-step guidance on building the Docker image, publishing to ECR, and running on ECS/Fargate with persistent storage and secrets.