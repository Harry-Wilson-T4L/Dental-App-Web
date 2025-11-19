[Reflection.Assembly]::LoadWithPartialName("System.IO.Compression") | Out-Null

function Write-AppSettings
{
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
    Param
    (
        [Parameter(Mandatory=$true, Position=0)]
        [String]
        $Path,

        [Parameter(Mandatory=$true, Position=1)]
        [String]
        $Config
    )
    Process
    {
        $encoding = New-Object System.Text.UTF8Encoding $false
        $writer = New-Object System.IO.StreamWriter $Path,$false,$encoding
        try
        {
            $writer.Write($Config)
            $writer.Flush()
        }
        finally
        {
            $writer.Dispose()
        }
    }
}

function Get-ArchiveRoot
{
    [CmdletBinding()]
    Param
    (
        [Parameter(Mandatory=$true, Position=0)]
        [String]
        $ArchivePath
    )
    
    $fs = New-Object System.IO.FileStream $ArchivePath,Open,Read
    try
    {
        $zip = New-Object System.IO.Compression.ZipArchive $fs,Read,$true
        try
        {
            foreach ($entry in $zip.Entries)
            {
                if (($entry.Name -eq "appsettings.json" -and $entry.FullName.Contains("bin/Release/net")))
                {
                    $fullName = $entry.FullName
                    if ([IO.Path]::DirectorySeparatorChar -eq '\') {
                        $fullName = $fullName.Replace("/", "\")
                    }

                    return [IO.Path]::GetDirectoryName($fullName)
                }
            }

            foreach ($entry in $zip.Entries)
            {
                if (($entry.Name -eq "appsettings.json" -and $entry.FullName.Contains("obj/Release/net")))
                {
                    $fullName = $entry.FullName
                    if ([IO.Path]::DirectorySeparatorChar -eq '\') {
                        $fullName = $fullName.Replace("/", "\")
                    }
                    
                    return [IO.Path]::GetDirectoryName($fullName)
                }
            }
        }
        finally
        {
            $zip.Dispose()
        }
    }
    finally
    {
        $fs.Close()
    }
}

function Extract-ArchiveRelative
{
    [CmdletBinding()]
    Param
    (
        [String]
        $ArchivePath,

        [String]
        $Destination,

        [String]
        $RelativeRoot
    )
    Process
    {
        $ArchivePath = $ArchivePath.Replace("\", "/")
        $RelativeRoot = $RelativeRoot.Replace("\", "/")
        if (-not $RelativeRoot.EndsWith("/")) {
            $RelativeRoot = $RelativeRoot + "/"
        }
        $Destination = $Destination.Replace("\", "/")

        Write-Verbose "Opening ZipArchive $ArchivePath"
        $fs = New-Object System.IO.FileStream $ArchivePath,Open,Read
        try
        {
            $zip = New-Object System.IO.Compression.ZipArchive $fs,Read,$true
            try
            {
                foreach ($entry in $zip.Entries)
                {
                    Write-Verbose "Processing entry $($entry.FullName)"
                    if ($entry.FullName.StartsWith($RelativeRoot, $true, [cultureinfo]::InvariantCulture)) {
                        $RelativePath = $entry.FullName.Substring($RelativeRoot.Length)
                        Write-Verbose "  RelativePath $RelativePath"
                        if ($RelativePath -ne $null -and $RelativePath -ne "" -and (-not $RelativePath.EndsWith("/"))) {
                            $DestinationFile = [IO.Path]::Combine($Destination, $RelativePath)
                            Write-Verbose "  DestinationFile $DestinationFile"
                            
                            $DestinationFileDir = [IO.Path]::GetDirectoryName($DestinationFile)
                            Write-Verbose "  DestinationFileDir $DestinationFileDir"
                            
                            if (-not [IO.Directory]::Exists($DestinationFileDir)) {
                                [IO.Directory]::CreateDirectory($DestinationFileDir) | Out-Null
                            }

                            $DestinationStream = New-Object System.IO.FileStream $DestinationFile,Create,ReadWrite
                            try
                            {
                                $EntryStream = $entry.Open()
                                try
                                {
                                    $EntryStream.CopyTo($DestinationStream)
                                }
                                finally
                                {
                                    $EntryStream.Dispose()
                                }
                            }
                            finally
                            {
                                $DestinationStream.Close()
                            }
                        }
                    }
                }
            }
            finally
            {
                $zip.Dispose()
            }
        }
        finally
        {
            $fs.Close()
        }
    }
}

function Encrypt-Config
{
    Param
    (
        [Parameter(Mandatory=$true, Position=0)]
        [String]
        $Environment
    )
    Begin
    {
        Import-Module -DisableNameChecking "$PSScriptRoot\Encryption-Tools.psm1"
    }
    Process
    {
        $rootDirectory = [IO.Path]::GetDirectoryName($PSScriptRoot)
        $envDirectory = [IO.Path]::Combine($rootDirectory, $Environment)
        if (-not [IO.Directory]::Exists($envDirectory)) 
        {
            throw "Environment $Environment is not defined"
        }

        $originalPath = [IO.Path]::Combine($envDirectory, "Secure-Config.ps1")
        if (-not [IO.File]::Exists($originalPath))
        {
            throw "Original config is not found in environment $Environment"
        }

        $password = Read-Host -Prompt "Encryption password"
        $encryptedPath = [IO.Path]::Combine($envDirectory, "Secure-Config.encrypted")

        if ([IO.File]::Exists($encryptedPath))
        {
            [IO.File]::Delete($encryptedPath)
        }

        Encrypt-File -Algorithm "aes-256-cbc" -Source $originalPath -Destination $encryptedPath -Password $password
    }
}

function Decrypt-Config
{
    Param
    (
        [Parameter(Mandatory=$true, Position=0)]
        [String]
        $Environment
    )
    Begin
    {
        Import-Module -DisableNameChecking "$PSScriptRoot\Encryption-Tools.psm1"
    }
    Process
    {
        $rootDirectory = [IO.Path]::GetDirectoryName($PSScriptRoot)
        $envDirectory = [IO.Path]::Combine($rootDirectory, $Environment)
        if (-not [IO.Directory]::Exists($envDirectory)) 
        {
            throw "Environment $Environment is not defined"
        }

        $encryptedPath = [IO.Path]::Combine($envDirectory, "Secure-Config.encrypted")
        if (-not [IO.File]::Exists($encryptedPath))
        {
            throw "Encrypted config is not found in environment $Environment"
        }

        $password = Read-Host -Prompt "Encryption password"
        $originalPath = [IO.Path]::Combine($envDirectory, "Secure-Config.ps1")

        if ([IO.File]::Exists($originalPath))
        {
            [IO.File]::Delete($originalPath)
        }

        Decrypt-File -Algorithm "aes-256-cbc" -Source $encryptedPath -Destination $originalPath -Password $password
    }
}