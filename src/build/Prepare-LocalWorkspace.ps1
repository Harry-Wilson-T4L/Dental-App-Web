Begin
{
    function Ready-LocalTools
    {
        [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
        Param()
        Process
        {
            $localToolsConfiguration = [IO.Path]::Combine($PSScriptRoot, "Configure-LocalTools.ps1")
            $localToolsTemplate = [IO.Path]::Combine($PSScriptRoot, "Configure-LocalTools-Template.psm1")

            if (-not [IO.File]::Exists($localToolsConfiguration))
            {
                Copy-Item -Path $localToolsTemplate -Destination $localToolsConfiguration
            }

            & $localToolsConfiguration
        }
    }

    function Drop-Artifact
    {
        [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
        Param
        (
            [Parameter(Mandatory = $true, Position = 0)]
            [String]
            $Path,

            [Parameter(Mandatory = $true, Position = 1)]
            [String]
            $Name,

            [Parameter(Mandatory = $true, Position = 2)]
            [String]
            $TestFile
        )
        Process
        {
            $SourcePath = [IO.Path]::Combine($solutionRoot, $Path) + "\"
            $TargetPath = [IO.Path]::Combine($releaseDirectory, "Published", $Name) + "\"
            $TestPath = [IO.Path]::Combine($solutionRoot, $TestFile)
            
            if ([IO.File]::Exists($TestPath)) {
                Copy-Item -Path $SourcePath -Recurse -Destination $TargetPath
            } else {
                throw new "$TestFile not found"
            }
        }
    }

    Ready-LocalTools
}
Process
{
    $currentDirectory = $PSScriptRoot

    $solutionRoot = [IO.Path]::GetDirectoryName($currentDirectory)

    Write-Verbose "Solution Root: $solutionRoot"

    $releaseDirectory = [IO.Path]::Combine($solutionRoot, "release")
    if ([IO.Directory]::Exists($releaseDirectory)) {
        [IO.Directory]::Delete($releaseDirectory, $true)
    }

    [IO.Directory]::CreateDirectory($releaseDirectory) | Out-Null

    Drop-Artifact -Path "publish\package" -Name "drop" -TestFile "publish\package\DentalDrill.CRM.zip"
    Drop-Artifact -Path "build" -Name "build" -TestFile "build\scripts\shared\Build-Tools.psm1"

    Set-Location -Path $solutionRoot

    $gitLastCommit = (git log -n 1 --pretty=format:"%h")
    $gitStatusChanges = (git status --porcelain)
    if (-not ($gitStatusChanges -eq $null -or $gitStatusChanges -eq ""))
    {
        $gitLastCommit = "$gitLastCommit*"
    }

    Set-Location $releaseDirectory

    Set-Item Env:\PublishName -Value "DentalDrill.CRM"

    Set-Item Env:\System_DefaultWorkingDirectory -Value $releaseDirectory
    Set-Item Env:\Release_Artifacts_Published_BuildNumber -Value "ManualBuild"
    Set-Item Env:\Release_Artifacts_Published_SourceVersion -Value $gitLastCommit
}