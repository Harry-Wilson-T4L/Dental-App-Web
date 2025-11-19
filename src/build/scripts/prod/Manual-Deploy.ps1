Begin
{
    Import-Module -DisableNameChecking "$env:System_DefaultWorkingDirectory\Published\build\scripts\shared\Release-Tasks.psm1" -Force

    function Prepare-WebSiteConfig
    {
        [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
        Param()
        Process
        {
            $script = "$env:System_DefaultWorkingDirectory/Published/build/scripts/prod/Create-Config.ps1"
            & $script -SecureConfigPassword $Params_SecureConfigPassword
        }
    }

    function Embed-WebSiteConfig
    {
        [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
        Param()
        Process
        {
            $folder = "$env:System_DefaultWorkingDirectory/Configs/Content"
            $archive = "$env:System_DefaultWorkingDirectory/Published/drop/$env:PublishName.zip"

            Archive-Files -Folder $folder -Archive $archive
        }
    }

    function Set-DeployParameters
    {
        Param()
        Process
        {
            $drop = "$env:System_DefaultWorkingDirectory\Published\drop"
            $xml = [xml](Get-Content "$drop\$env:PublishName.SetParameters.xml")
            $node = $xml.SelectSingleNode("/parameters/setParameter[@name='IIS Web Application Name']/@value")
            if ($node -eq $null) {
                throw "Invalid configuration"
            }

            $node.Value = $env:PublishSiteName
            $xml.Save("$drop\$env:PublishName.SetParameters.xml")
            Write-Host $xml.OuterXml
        }
    }

    function Perform-Deploy
    {
        [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
        Param()
        Process
        {
            $command = "$env:System_DefaultWorkingDirectory/Published/drop/$env:PublishName.deploy.cmd"
            $arguments = "/Y ""/M:$($env:PublishServiceUrl)?site=$($env:PublishSiteName)"" /U:$env:PublishUsername /P:$Params_PublishPassword /A:Basic -enableRule:AppOffline -usechecksum"

            Write-Output "Execute $command $arguments"
        }
    }
}
Process
{
    $Params_SecureConfigPassword = Read-Host "SecureConfigPassword"
    $Params_PublishServiceUrl = Read-Host "PublishServiceUrl"
    $Params_PublishSiteName = Read-Host "PublishSiteName"
    $Params_PublishUsername = Read-Host "PublishUsername"
    $Params_PublishPassword = Read-Host "PublishPassword"

    Set-Item Env:\PublishServiceUrl -Value $Params_PublishServiceUrl
    Set-Item Env:\PublishSiteName -Value $Params_PublishSiteName
    Set-Item Env:\PublishUsername -Value $Params_PublishUsername
    
    Prepare-WebSiteConfig
    Embed-WebSiteConfig
    Set-DeployParameters
    Perform-Deploy
}
End
{
}