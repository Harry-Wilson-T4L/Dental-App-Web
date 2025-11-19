[CmdletBinding()]
Param()
Begin
{
    $ErrorActionPreference = "Stop"
    Import-Module -DisableNameChecking "$env:SYSTEM_DEFAULTWORKINGDIRECTORY/Published/build/scripts/shared/Encryption-Tools.psm1"
    Import-Module -DisableNameChecking "$env:SYSTEM_DEFAULTWORKINGDIRECTORY/Published/build/scripts/shared/Build-Tools.psm1"
}
Process
{
    # Configuration
    $ArtifactName = "drop"
    $ContainerName = "app"
    $ArchiveName = "DentalDrill.CRM"
    # End of configuration

    # Preparation

    # Calculating paths
    $ArtifactRoot = [IO.Path]::Combine($env:SYSTEM_DEFAULTWORKINGDIRECTORY, "Published", $ArtifactName)

    $ArchivePath = [IO.Path]::Combine($ArtifactRoot, "$ArchiveName.zip")
    Write-Verbose "ArchivePath: $ArchivePath"

    $ArchiveRoot = Get-ArchiveRoot $ArchivePath
    Write-Verbose "ArchiveRoot: $ArchiveRoot"

    $DockerRoot = [IO.Path]::Combine($env:SYSTEM_DEFAULTWORKINGDIRECTORY, "Container", $ContainerName)
    Write-Verbose "DockerRoot: $DockerRoot"

    $DockerSourceRoot = [IO.Path]::Combine($DockerRoot, "app")
    Write-Verbose "DockerSourceRoot: $DockerSourceRoot"

    # End of preparation

    # Execution
    Extract-ArchiveRelative -ArchivePath $ArchivePath -Destination $DockerSourceRoot -RelativeRoot $ArchiveRoot

    # Removing application configuration files
    # Configuration will have to be supplied via docker-entrypoint.sh
    Remove-Item -Path ([IO.Path]::Combine($DockerSourceRoot, "appsettings.json")) -Force
    Remove-Item -Path ([IO.Path]::Combine($DockerSourceRoot, "appsettings.Development.json")) -Force
    Remove-Item -Path ([IO.Path]::Combine($DockerSourceRoot, "appsettings.User.json")) -Force

    # Removing other development configuration files that are not required for an app to run
    Remove-Item -Path ([IO.Path]::Combine($DockerSourceRoot, "bower.json")) -Force
    Remove-Item -Path ([IO.Path]::Combine($DockerSourceRoot, "package-lock.json")) -Force
    Remove-Item -Path ([IO.Path]::Combine($DockerSourceRoot, "tsconfig.json")) -Force
    Remove-Item -Path ([IO.Path]::Combine($DockerSourceRoot, "web.config")) -Force

    # Removing directories that are supposed to be mounted via volumes
    Remove-Item -Path ([IO.Path]::Combine($DockerSourceRoot, "wwwroot", "images")) -Force -Recurse
    Remove-Item -Path ([IO.Path]::Combine($DockerSourceRoot, "wwwroot", "files")) -Force -Recurse

    Copy-Item -Path ([IO.Path]::Combine($PSScriptRoot, "docker-entrypoint.sh")) -Destination ([IO.Path]::Combine($DockerSourceRoot, "docker-entrypoint.sh"))
    Copy-Item -Path ([IO.Path]::Combine($PSScriptRoot, "Dockerfile")) -Destination ([IO.Path]::Combine($DockerRoot, "Dockerfile"))

    Write-AppSettings -Path ([IO.Path]::Combine($DockerSourceRoot, "appsettings.BuildInfo.json")) -Config `
    "{
        ""Application"": {
            ""Build"": ""$($env:RELEASE_ARTIFACTS_PUBLISHED_BUILDNUMBER)"",
            ""Commit"": ""$($env:RELEASE_ARTIFACTS_PUBLISHED_SOURCEVERSION)""
        }
    }"

    # End of execution
}