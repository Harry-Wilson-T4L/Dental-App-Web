## DentalDrill.CRM – Local IIS Deployment Guide

### 1. Prerequisites
- **Windows features**: Internet Information Services, IIS Management Console, .NET Extensibility 4.8+, ASP.NET 4.8, WebSocket Protocol (optional but recommended).
- **.NET SDK/Runtime**: .NET 6 runtime available on the server (matches project `net6.0` Target Framework).
- **Node.js**: Installed so `npm install` can run for the web assets.
- **SQL Server**: Instance reachable and connection string prepared for `appsettings.json`.

### 2. Build & Publish
1. Open a Developer PowerShell in `D:\Hassan Project_Code\DentalDrill.CRM-Export-20241204\src`.
2. Restore tools  
   ```powershell
   dotnet restore DentalDrill.CRM.sln
   cd DentalDrill.CRM
   npm install
   cd ..
   ```
3. Publish Release output to a folder (adjust path if desired):  
   ```powershell
   dotnet publish DentalDrill.CRM\DentalDrill.CRM.csproj -c Release -o publish\DentalDrill.CRM
   ```
   Resulting folder contains compiled binaries, static assets, and `web.config`.

### 3. Configure IIS Site
1. Open **IIS Manager** → **Sites** → **Add Website…**
   - **Site name**: `DentalDrill.CRM`
   - **Physical path**: `D:\Hassan Project_Code\DentalDrill.CRM-Export-20241204\src\publish\DentalDrill.CRM`
   - **Binding**: choose `http`, IP `All Unassigned`, Port `8080` (or 80 if unused), Host name as needed.
2. Ensure **Application Pool**:
   - Uses **No Managed Code** (Kestrel-hosted).
   - **Enable 32-bit**: `False`.
   - **Start mode**: `AlwaysRunning` if you need faster cold starts.
3. Set folder permissions so the IIS AppPool identity (e.g., `IIS AppPool\DentalDrill.CRM`) has **Read/Execute** on the publish folder and **Modify** on any directories that need write access (e.g., `wwwroot\files`, logs, uploads).

### 4. Configure App Settings
1. Copy `DentalDrill.CRM/appsettings.json` and `appsettings.Development.json` beside the published files if required.
2. Create or edit `appsettings.Production.json` (optional) with production connection strings, email/SMS credentials, etc.
3. Alternatively, set connection strings and secrets in IIS via **Configuration Editor** → `appSettings`, or use environment variables (preferred for secrets).

### 5. Database / EF Migrations
1. Update the connection string to point to your SQL Server.
2. Apply migrations from the solution root:  
   ```powershell
   dotnet ef database update --project DentalDrill.CRM\DentalDrill.CRM.csproj
   ```
   (Requires `Microsoft.EntityFrameworkCore.Tools` and `appsettings.json` connection string configured.)

### 6. Verify
1. Browse `http://localhost:8080` (or chosen binding) to confirm the site starts.
2. Check Windows Event Viewer → **Application** and `D:\...\publish\DentalDrill.CRM\logs` (if configured) for errors.
3. If static assets fail to load, confirm `wwwroot` content exists in the publish folder and MIME types are allowed.

### 7. Maintenance Tips
- Re-run `npm install` + `dotnet publish` whenever package.json changes.
- Keep .NET runtime updated; project currently targets `net6.0` (out of support), consider upgrading to `net8.0`.
- Monitor NuGet vulnerabilities flagged for `MimeKit 4.0.0` and `SkiaSharp 2.88.3`; plan package upgrades.
- Back up `appsettings.*.json` and database before deploying updates.

Following these steps lets you replicate the build locally, deploy to IIS, and document the environment for other team members.

