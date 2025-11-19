function Archive-Files
{
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String]
        $Folder,

        [Parameter(Mandatory = $true, Position = 1)]
        [String]
        $Archive
    )

    Process
    {
        $tool = $env:ReleaseTools_SevenZipExe
        & $tool a -tzip $Archive $Folder
    }
}