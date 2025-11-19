Begin
{
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
}