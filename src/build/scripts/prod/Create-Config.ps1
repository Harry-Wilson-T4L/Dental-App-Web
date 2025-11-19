Param
(
    [Parameter(Mandatory=$true, Position=0)]
    $SecureConfigPassword
)
Begin
{
    $ErrorActionPreference = "Stop"
    Import-Module -DisableNameChecking "$env:System_DefaultWorkingDirectory\Published\build\scripts\shared\Encryption-Tools.psm1"
    Import-Module -DisableNameChecking "$env:System_DefaultWorkingDirectory\Published\build\scripts\shared\Build-Tools.psm1"
}
Process
{
    $Environment = "prod"

    $DropRoot = [IO.Path]::Combine($env:System_DefaultWorkingDirectory, "Published", "drop")

    # Adding skip rules to prevent content deletion
    $DeployCmdPath = [IO.Path]::Combine($DropRoot, "$env:PublishName.deploy.cmd")
    $DeployCmd =  [IO.File]::ReadAllText($DeployCmdPath, [System.Text.Encoding]::ASCII)
    $OldArgsString = "-disableLink:AppPoolExtension -disableLink:ContentExtension -disableLink:CertificateExtension"
    $NewArgsString = $OldArgsString + " -skip:skipaction='Delete',objectname='dirPath',absolutepath=""^[^\\]*\\wwwroot\\images\\.*"""
    $NewArgsString = $NewArgsString + " -skip:skipaction='Delete',objectname='filePath',absolutepath=""^[^\\]*\\wwwroot\\images\\.*"""
    $NewArgsString = $NewArgsString + " -skip:skipaction='Delete',objectname='dirPath',absolutepath=""^[^\\]*\\wwwroot\\files\\.*"""
    $NewArgsString = $NewArgsString + " -skip:skipaction='Delete',objectname='filePath',absolutepath=""^[^\\]*\\wwwroot\\files\\.*"""
    $NewArgsString = $NewArgsString + " -skip:skipaction='Delete',objectname='dirPath',absolutepath=""^[^\\]*\\logs($|\\.*)"""
    $NewArgsString = $NewArgsString + " -skip:skipaction='Delete',objectname='filePath',absolutepath=""^[^\\]*\\logs\\.*"""
    $DeployCmd = $DeployCmd.Replace($OldArgsString, $NewArgsString)
    [IO.File]::WriteAllText($DeployCmdPath, $DeployCmd, [System.Text.Encoding]::ASCII)
    # End of adding skip rules

    $ArchivePath = [IO.Path]::Combine($DropRoot, "$env:PublishName.zip")
    $ArchiveRoot = Get-ArchiveRoot $ArchivePath

    $ConfigRoot = [IO.Path]::Combine($env:System_DefaultWorkingDirectory, "Configs", $ArchiveRoot)
    [IO.Directory]::CreateDirectory($ConfigRoot)

    # Getting paths to appsettings.json
    $AppConfig = [IO.Path]::Combine($ConfigRoot, "appsettings.json")

    # Loading secure config
    $SecureConfigPath = "$env:System_DefaultWorkingDirectory\Published\build\scripts\$Environment\Secure-Config.encrypted"
    $SecureConfigDecrypted = Decrypt-Base64ToString -Algorithm "aes-256-cbc" -Base64String ([Convert]::ToBase64String([IO.File]::ReadAllBytes($SecureConfigPath))) -Password $SecureConfigPassword
    $SecureConfig = Invoke-Expression -Command $SecureConfigDecrypted

    $WebConfigSourcePath = "$env:System_DefaultWorkingDirectory\Published\build\scripts\$Environment\web.config"
    $WebConfigDestinationPath = [IO.Path]::Combine($ConfigRoot, "web.config")
    Copy-Item -Path $WebConfigSourcePath -Destination $WebConfigDestinationPath

    Write-AppSettings -Path $AppConfig -Config `
"{
    ""ConnectionStrings"": {
        ""DefaultConnection"": ""Server=.\\SQLEXPRESS;Database=DentalDrill.CRM.Production;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true""
    },
    ""Application"": {
        ""BaseUrl"": ""https://hub.dentaldrillsolutions.com.au/""
    },
    ""PickupRequest"": {
        ""Email"": {
            ""Enabled"": true,
            ""Recipient"": ""info@dentaldrillsolutions.com.au, sydney@dentaldrillsolutions.com.au, caroline@dentaldrillsolutions.com.au, office@dentaldrillsolutions.com.au"",
            ""BaseUrl"": ""https://hub.dentaldrillsolutions.com.au/"",
            ""SubjectPrefix"": """"
        },
        ""SmartFreight"": {
            ""Enabled"": true,
            ""Testing"": false,
            ""SenderEmail"": ""info@dentaldrillsolutions.com.au"",
            ""ServiceUri"": ""https://api-r1.smartfreight.com/api/soap/classic"",
            ""Id"": ""$($SecureConfig.SmartFreight.Username)"",
            ""Password"": ""$($SecureConfig.SmartFreight.Password)""
        }
    },
    ""HandpieceStoreOrders"": {
        ""Email"": {
            ""Enabled"": true,
            ""Recipient"": ""info@dentaldrillsolutions.com.au"",
            ""BaseUrl"": ""https://hub.dentaldrillsolutions.com.au/"",
            ""SubjectPrefix"": """"
        }
    },
    ""MaintenanceReminder"": {
        ""Enabled"": false,
        ""Schedule"": ""0 0 12 * * ?""
    },
    ""Bundling"": {
        ""Enabled"": true
    },
    ""Calendar"": {
        ""TimeZone"": ""AUS Eastern Standard Time""
    },
    ""Themes"": {
        ""StaffTheme"": ""light2""
    },
    ""Storage"": {
        ""Images"": {
            ""Type"": ""AmazonS3"",
            ""BucketName"": ""hub-dentaldrillsolutions"",
            ""Region"": ""ap-southeast-2"",
            ""Key"": ""Images/"",
            ""AccessKey"": ""$($SecureConfig.Amazon.AccessKey)"",
            ""SecretKey"": ""$($SecureConfig.Amazon.SecretKey)"",
            ""PublicRead"": true
        },
        ""Files"": {
            ""Type"": ""AmazonS3"",
            ""BucketName"": ""hub-dentaldrillsolutions"",
            ""Region"": ""ap-southeast-2"",
            ""Key"": ""Files/"",
            ""AccessKey"": ""$($SecureConfig.Amazon.AccessKey)"",
            ""SecretKey"": ""$($SecureConfig.Amazon.SecretKey)"",
            ""PublicRead"": true
        },
        ""Emails"": {
            ""Type"": ""AmazonS3"",
            ""BucketName"": ""hub-dentaldrillsolutions-emails"",
            ""Region"": ""ap-southeast-2"",
            ""Key"": ""Emails/"",
            ""AccessKey"": ""$($SecureConfig.Amazon.AccessKey)"",
            ""SecretKey"": ""$($SecureConfig.Amazon.SecretKey)"",
            ""PublicRead"": false
        }
    },
    ""Mail"": {
        ""System"": {
            ""Type"": ""AmazonSes"",
            ""Sender"": ""info@dentaldrillsolutions.com.au"",
            ""DebugMode"": false,
            ""Options"": {
                ""Region"": ""eu-west-1"",
                ""AccessKey"": ""$($SecureConfig.Amazon.AccessKey)"",
                ""SecretKey"": ""$($SecureConfig.Amazon.SecretKey)""
            }
        }
    },
    ""Sms"": {
        ""System"": {
            ""Type"": ""None"",
            ""SenderName"": ""DevGuild"" 
        } 
    },
    ""Security"": {
        ""Hsts"": {
            ""Enabled"": true,
            ""Preload"": false,
            ""IncludeSubDomains"": false,
            ""MaxAge"": ""30.00:00:00""
        },
        ""HttpsRedirection"": {
            ""Enabled"": true,
            ""HttpsPort"": 443,
            ""RedirectStatusCode"": 301 
        } 
    },
    ""Logging"": {
        ""IncludeScopes"": false,
        ""LogLevel"": {
            ""Default"": ""Warning""
        },
        ""Repository"": {
            ""LogLevel"": {
                ""Default"": ""None"",
                ""DentalDrill"": ""Information""
            }
        }
    }
}"
}
End
{

}