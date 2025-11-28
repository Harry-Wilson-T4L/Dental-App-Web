# DentalDrill.CRM Build Fixes and Setup Guide

This document outlines all the build issues that were fixed and provides step-by-step instructions for resolving them.

## Table of Contents

1. [Overview](#overview)
2. [Setup Scripts](#setup-scripts)
3. [Fixed Issues](#fixed-issues)
   - [Missing npm Packages](#1-missing-npm-packages)
   - [Missing culture.js File](#2-missing-culturejs-file)
   - [TypeScript Module Resolution Error](#3-typescript-module-resolution-error)
4. [Step-by-Step Fix Instructions](#step-by-step-fix-instructions)
5. [Prevention and Best Practices](#prevention-and-best-practices)

---

## Overview

This project requires both .NET packages (NuGet) and JavaScript/TypeScript packages (npm) to build successfully. The main issues encountered were:

- Missing npm packages for Kendo UI and TypeScript
- TypeScript compilation errors
- Module resolution configuration issues

---

## Setup Scripts

### Automated Setup (Recommended)

A setup script has been created to automate the entire setup process:

**File:** `setup.bat` (double-click to run)

This script:
- ✓ Checks prerequisites (.NET SDK, Node.js, npm)
- ✓ Installs all npm packages in `src/` directory
- ✓ Installs all npm packages in `src/DentalDrill.CRM/` directory
- ✓ Restores all NuGet packages
- ✓ Builds the solution to verify everything works

**Usage:**
1. Clone the repository
2. Double-click `setup.bat` in the root directory
3. Wait for the script to complete
4. Open `src/DentalDrill.CRM.sln` in Visual Studio
5. Build and run (F5)

---

## Fixed Issues

### 1. Missing npm Packages

**Error:** Build fails because TypeScript, Kendo UI, and other npm packages are not installed.

**Root Cause:** The `node_modules` directory was missing, so TypeScript compiler and Kendo UI types were not available.

**Solution:** Install all required npm packages.

**Files Modified:**
- None (packages are installed, not committed to git)

**Required Packages:**
- `typescript@^4.9.5` - TypeScript compiler
- `@progress/kendo-ui@^2025.4.1111` - Kendo UI library
- `@types/kendo-ui@^2023.2.6` - Kendo UI type definitions
- `@types/jquery@^3.5.33` - jQuery type definitions
- `@types/jqueryui@^1.12.24` - jQuery UI type definitions
- `@types/bootstrap@^5.2.10` - Bootstrap type definitions

---

### 2. Missing culture.js File

**Error:**
```
System.InvalidOperationException: No file exists for the asset at either location 
'D:\...\wwwroot\js\app\config\culture.js' or 'wwwroot\js\app\config\culture.js'.
```

**Root Cause:** 
- The `culture.ts` file existed but wasn't compiling to `culture.js`
- The file used ES6 `import` syntax which conflicted with the project's TypeScript configuration (`module: "none"`)

**Solution:** 
1. Removed the ES6 import statement (Kendo culture file is already loaded in the bundle)
2. Updated the file to use the global `kendo` object directly
3. Updated TypeScript configuration to properly compile the file

**Files Modified:**
- `src/DentalDrill.CRM/wwwroot/js/app/config/culture.ts`
- `src/DentalDrill.CRM/tsconfig.json`
- `src/DentalDrill.CRM/DentalDrill.CRM.csproj`

**Before:**
```typescript
import '@progress/kendo-ui/js/cultures/kendo.culture.en-AU';

(() => {
    kendo.culture("en-AU");
})();
```

**After:**
```typescript
(() => {
    // Kendo culture file is already loaded in the bundle before this file
    if (typeof kendo !== 'undefined') {
        kendo.culture("en-AU");
    }
})();
```

---

### 3. TypeScript Module Resolution Error

**Error:**
```
Build: Cannot find module '@popperjs/core'. Did you mean to set the 'moduleResolution' 
option to 'node', or to add aliases to the 'paths' option?
```

**Root Cause:** 
- TypeScript configuration had `module: "none"` but was missing `moduleResolution` setting
- TypeScript couldn't resolve module references from type definition files (like Bootstrap types that reference `@popperjs/core`)

**Solution:** 
Added `moduleResolution: "node"` and `skipLibCheck: true` to `tsconfig.json`

**Files Modified:**
- `src/DentalDrill.CRM/tsconfig.json`

**Changes:**
```json
{
  "compilerOptions": {
    // ... existing options ...
    "module": "none",
    "moduleResolution": "node",  // Added
    "skipLibCheck": true,         // Added
    // ... rest of options ...
  }
}
```

---

## Step-by-Step Fix Instructions

### For New Developers (First Time Setup)

1. **Install Prerequisites:**
   ```powershell
   # Check if .NET SDK is installed
   dotnet --version
   
   # Check if Node.js is installed
   node --version
   
   # Check if npm is installed
   npm --version
   ```

2. **Run Setup Script:**
   - Double-click `setup.bat` in the root directory
   - OR run manually:
     ```powershell
     cd src
     npm install
     cd DentalDrill.CRM
     npm install
     cd ..
     dotnet restore
     dotnet build
     ```

3. **Open in Visual Studio:**
   - Open `src/DentalDrill.CRM.sln`
   - Set `DentalDrill.CRM` as startup project
   - Press F5 to build and run

### Manual Fix for Missing npm Packages

If you encounter missing package errors:

1. **Navigate to project directory:**
   ```powershell
   cd src/DentalDrill.CRM
   ```

2. **Install packages:**
   ```powershell
   npm install
   ```

3. **Also install in src directory (if needed):**
   ```powershell
   cd ..
   npm install
   ```

4. **Verify packages are installed:**
   ```powershell
   Test-Path node_modules\typescript
   Test-Path node_modules\@progress\kendo-ui
   Test-Path node_modules\@types\kendo-ui
   ```

### Manual Fix for culture.js Error

If you get the `culture.js` missing error:

1. **Check if culture.ts exists:**
   ```powershell
   Test-Path "wwwroot\js\app\config\culture.ts"
   ```

2. **Verify the file content** (should not have ES6 imports):
   ```typescript
   (() => {
       if (typeof kendo !== 'undefined') {
           kendo.culture("en-AU");
       }
   })();
   ```

3. **Compile TypeScript manually (if needed):**
   ```powershell
   node_modules\.bin\tsc.cmd wwwroot\js\app\config\culture.ts --outDir wwwroot\js\app\config --target es5 --module none --sourceMap
   ```

4. **Verify culture.js was created:**
   ```powershell
   Test-Path "wwwroot\js\app\config\culture.js"
   ```

5. **Build the project:**
   ```powershell
   dotnet build
   ```

### Manual Fix for Module Resolution Error

If you get `@popperjs/core` or similar module resolution errors:

1. **Open `tsconfig.json`**

2. **Add the following to `compilerOptions`:**
   ```json
   {
     "compilerOptions": {
       "moduleResolution": "node",
       "skipLibCheck": true,
       // ... other options
     }
   }
   ```

3. **Save and rebuild:**
   ```powershell
   dotnet clean
   dotnet build
   ```

---

## Configuration Files Summary

### Modified Files

1. **`src/DentalDrill.CRM/tsconfig.json`**
   - Added `"moduleResolution": "node"`
   - Added `"skipLibCheck": true`
   - Changed `"module": "none"` (was empty)

2. **`src/DentalDrill.CRM/wwwroot/js/app/config/culture.ts`**
   - Removed ES6 import statement
   - Added safety check for `kendo` object

3. **`src/DentalDrill.CRM/DentalDrill.CRM.csproj`**
   - Set `TypeScriptModuleKind` to `None`

4. **`setup.ps1`** (New)
   - Automated setup script

5. **`setup.bat`** (New)
   - Batch file wrapper for easy execution

6. **`README.md`**
   - Updated with quick setup instructions

### Key Configuration Values

**TypeScript Configuration (`tsconfig.json`):**
```json
{
  "compileOnSave": true,
  "compilerOptions": {
    "target": "es5",
    "module": "none",
    "moduleResolution": "node",
    "skipLibCheck": true,
    "noEmitOnError": true,
    "sourceMap": true
  }
}
```

**Project File (`DentalDrill.CRM.csproj`):**
```xml
<PropertyGroup>
  <TypeScriptTarget>ES5</TypeScriptTarget>
  <TypeScriptModuleKind>None</TypeScriptModuleKind>
  <TypeScriptCompileOnSaveEnabled>True</TypeScriptCompileOnSaveEnabled>
  <TypeScriptNoEmitOnError>True</TypeScriptNoEmitOnError>
</PropertyGroup>
```

---

## Prevention and Best Practices

### For Developers

1. **Always run `setup.bat` after cloning:**
   - Ensures all packages are installed
   - Verifies the build works

2. **Don't commit `node_modules`:**
   - Already in `.gitignore`
   - Each developer should run `npm install`

3. **TypeScript Files:**
   - Don't use ES6 `import` statements when `module: "none"` is set
   - Use global objects (like `kendo`, `$`) directly
   - Ensure files compile to `.js` in the same directory

4. **Before Committing:**
   - Run `dotnet build` to verify everything compiles
   - Check that all `.ts` files have corresponding `.js` files (if needed)

### For CI/CD

1. **Add npm install steps:**
   ```yaml
   - task: Npm@1
     inputs:
       command: 'install'
       workingDir: 'src'
   
   - task: Npm@1
     inputs:
       command: 'install'
       workingDir: 'src/DentalDrill.CRM'
   ```

2. **Build verification:**
   ```yaml
   - task: DotNetCoreCLI@2
     inputs:
       command: 'build'
       projects: 'src/DentalDrill.CRM.sln'
   ```

---

## Troubleshooting

### Issue: npm install fails with permission errors

**Solution:**
```powershell
# Run PowerShell as Administrator
Set-ExecutionPolicy -ExecutionPolicy RemoteSigned -Scope CurrentUser
npm install
```

### Issue: TypeScript files not compiling

**Check:**
1. Verify `Microsoft.TypeScript.MSBuild` package is installed
2. Check `tsconfig.json` exists and is valid JSON
3. Ensure `TypeScriptCompile` items are included in `.csproj`

**Solution:**
```powershell
# Clean and rebuild
dotnet clean
dotnet build
```

### Issue: Build succeeds but runtime errors

**Check:**
1. Verify all JavaScript files are generated from TypeScript
2. Check browser console for missing files
3. Ensure bundles are properly configured in `bundles.json`

### Issue: Kendo UI not found

**Solution:**
```powershell
# Verify Kendo UI is installed
Test-Path "node_modules\@progress\kendo-ui"

# Reinstall if missing
npm install @progress/kendo-ui@^2025.4.1111
```

---

## Verification Checklist

After setup, verify:

- [ ] `node_modules` exists in `src/DentalDrill.CRM/`
- [ ] `node_modules` exists in `src/`
- [ ] `culture.js` exists in `wwwroot/js/app/config/`
- [ ] `dotnet build` succeeds without errors
- [ ] Visual Studio builds without errors
- [ ] All TypeScript files compile successfully

---

## Additional Resources

- [TypeScript Configuration Options](https://www.typescriptlang.org/tsconfig)
- [Kendo UI Documentation](https://docs.telerik.com/kendo-ui)
- [.NET 6 Documentation](https://docs.microsoft.com/en-us/dotnet/core/)
- [npm Documentation](https://docs.npmjs.com/)

---

## Support

If you encounter issues not covered in this document:

1. Check the error message carefully
2. Verify all prerequisites are installed
3. Run `setup.bat` again
4. Check the project's `README.md` for additional setup instructions
5. Review Visual Studio's Error List for specific file/line errors

---

**Last Updated:** November 2024  
**Document Version:** 1.0

