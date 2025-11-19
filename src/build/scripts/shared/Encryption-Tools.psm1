[Reflection.Assembly]::LoadWithPartialName("System.IO.Compression") | Out-Null

function Generate-Salt
{
    [OutputType([Byte[]])]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [Int32]
        $Length
    )
    Begin
    {
    }
    Process
    {
        $rng = New-Object System.Security.Cryptography.RNGCryptoServiceProvider
        try 
        {
            $salt = New-Object System.Byte[] $Length
            $rng.GetBytes($salt)
            return $salt
        }
        finally 
        {
            $rng.Dispose()
        }
    }
    End
    {
    }
}

function Generate-EncryptionKey
{
    [OutputType([Byte[]])]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'Password')]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String]
        $Password,

        [Parameter(Mandatory = $true, Position = 1)]
        [Byte[]]
        $Salt,

        [Parameter(Mandatory = $true, Position = 2)]
        [Int32]
        $Length
    )
    Begin
    {
    }
    Process
    {
        $pbkdf = New-Object System.Security.Cryptography.Rfc2898DeriveBytes $Password,$Salt
        try
        {
            return $pbkdf.GetBytes($Length)
        }
        finally
        {
            $pbkdf.Dispose()
        }
    }
    End
    {
    }
}

function Merge-ByteArrays
{
    [OutputType([Byte[]])]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [Byte[]]
        $FirstArray,

        [Parameter(Mandatory = $true, Position = 1)]
        [Byte[]]
        $SecondArray
    )
    Process
    {
        $Result = New-Object System.Byte[] ($FirstArray.Length + $SecondArray.Length)
        [System.Array]::Copy($FirstArray, 0, $Result, 0, $FirstArray.Length)
        [System.Array]::Copy($SecondArray, 0, $Result, $FirstArray.Length, $SecondArray.Length)
        return $Result
    }
}

function Take-FromByteArray
{
    [OutputType([Byte[]])]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [Byte[]]
        $Array,

        [Parameter(Mandatory = $true, Position = 1)]
        [Int32]
        $Length
    )
    Process
    {
        $Result = New-Object System.Byte[] $Length
        [System.Array]::Copy($Array, 0, $Result, 0, $Result.Length)
        return $Result
    }
}

function Skip-FromByteArray
{
    [OutputType([Byte[]])]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [Byte[]]
        $Array,

        [Parameter(Mandatory = $true, Position = 1)]
        [Int32]
        $Length
    )
    Process
    {
        $RemainingLength = $Array.Length - $Length
        if ($RemainingLength -gt 0) 
        {
            $Result = New-Object System.Byte[] $RemainingLength
            [System.Array]::Copy($Array, $Length, $Result, 0, $Result.Length)
            return $Result
        }
        else
        {
            return New-Object System.Byte[] 0
        }
    }
}

function Create-SymmetricAlgorithm
{
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String]
        $Name
    )
    Process
    {
        switch ($Name.ToLowerInvariant())
        {
            "aes-256-cbc" {
                $alg = New-Object System.Security.Cryptography.AesManaged
                $alg.KeySize = 256
                $alg.Mode = [System.Security.Cryptography.CipherMode]::CBC
                return $alg
            }
            "aes-192-cbc" {
                $alg = New-Object System.Security.Cryptography.AesManaged
                $alg.KeySize = 192
                $alg.Mode = [System.Security.Cryptography.CipherMode]::CBC
                return $alg
            }
            "aes-128-cbc" {
                $alg = New-Object System.Security.Cryptography.AesManaged
                $alg.KeySize = 128
                $alg.Mode = [System.Security.Cryptography.CipherMode]::CBC
                return $alg
            }
            default {
                throw "Algorithm $Name is not supported"
            }
        }
    }
}

function Decrypt-Base64ToString
{
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'Password')]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String]
        $Algorithm,

        [Parameter(Mandatory = $true, Position = 1)]
        [String]
        $Password,

        [Parameter(Mandatory = $true, Position = 2)]
        [String]
        $Base64String
    )
    Process
    {
        $algorithmImpl = Create-SymmetricAlgorithm -Name $Algorithm
        try 
        {
            $allBytes = [Convert]::FromBase64String($Base64String)
            # Reading salt
            [Byte[]] $salt = Take-FromByteArray -Array $allBytes -Length 16
            $allBytes = Skip-FromByteArray -Array $allBytes -Length 16
            # Reading IV
            [Byte[]] $iv = Take-FromByteArray -Array $allBytes -Length ($algorithmImpl.BlockSize / 8)
            $allBytes = Skip-FromByteArray -Array $allBytes -Length ($algorithmImpl.BlockSize / 8)
            [Byte[]] $key = Generate-EncryptionKey -Password $Password -Salt $salt -Length ($algorithmImpl.KeySize / 8)

            $algorithmImpl.Key = $key
            $algorithmImpl.IV = $iv
            $decryptor = $algorithmImpl.CreateDecryptor($algorithmImpl.Key, $algorithmImpl.IV)
            try 
            {
                $decryptedStream = New-Object System.IO.MemoryStream
                try 
                {
                    $cryptoStream = New-Object System.Security.Cryptography.CryptoStream $decryptedStream,$decryptor,Write
                    try
                    {
                        $cryptoStream.Write($allBytes, 0, $allBytes.Length)
                        $cryptoStream.Flush()
                        $cryptoStream.FlushFinalBlock()
                    }
                    finally
                    {
                        $cryptoStream.Dispose()
                    }

                    return [System.Text.Encoding]::UTF8.GetString($decryptedStream.ToArray())
                }
                finally
                {
                    $decryptedStream.Dispose()
                }
            }
            finally
            {
                $decryptor.Dispose()
            }
        }
        finally
        {
            $algorithmImpl.Dispose()
        }
    }
}

function Encrypt-StringToBase64
{
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'Password')]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String]
        $Algorithm,

        [Parameter(Mandatory = $true, Position = 1)]
        [String]
        $Password,

        [Parameter(Mandatory = $true, Position = 2)]
        [String]
        $Text
    )
    Process
    {
        $algorithmImpl = Create-SymmetricAlgorithm -Name $Algorithm
        try 
        {
            $salt = Generate-Salt -Length 16
            $key = Generate-EncryptionKey -Password $Password -Salt $salt -Length ($algorithmImpl.KeySize / 8)
            $algorithmImpl.Key = $key
            
            $encryptor = $algorithmImpl.CreateEncryptor($algorithmImpl.Key, $algorithmImpl.IV)
            try 
            {
                $encryptedStream = New-Object System.IO.MemoryStream
                try 
                {
                    $cryptoStream = New-Object System.Security.Cryptography.CryptoStream $encryptedStream,$encryptor,Write
                    try
                    {
                        $bytesToEncrypt = [System.Text.Encoding]::UTF8.GetBytes($Text)
                        $cryptoStream.Write($bytesToEncrypt, 0, $bytesToEncrypt.Length)
                        $cryptoStream.FlushFinalBlock()
                    }
                    finally
                    {
                        $cryptoStream.Dispose()
                    }

                    $encryptedBytes = $encryptedStream.ToArray()

                    $resultBytes = Merge-ByteArrays -FirstArray $salt -SecondArray $algorithmImpl.IV
                    $resultBytes = Merge-ByteArrays -FirstArray $resultBytes -SecondArray $encryptedBytes
                    return [Convert]::ToBase64String($resultBytes)
                }
                finally
                {
                    $encryptedStream.Dispose()
                }
            }
            finally
            {
                $encryptor.Dispose()
            }
        }
        finally
        {
            $algorithmImpl.Dispose()
        }
    }
}

function Encrypt-File
{
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'Password')]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String]
        $Algorithm,
        
        [Parameter(Mandatory = $true, Position = 1)]
        [String]
        $Source,

        [Parameter(Mandatory = $true, Position = 2)]
        [String]
        $Destination,

        [Parameter(Mandatory = $true, Position = 3)]
        [String]
        $Password
    )
    Begin
    {
        $CurrentEnvironmentCurrentDirectory = [System.Environment]::CurrentDirectory
        [System.Environment]::CurrentDirectory = Get-Location
    }
    Process
    {
        $algorithmImpl = Create-SymmetricAlgorithm -Name $Algorithm
        try 
        {
            $salt = Generate-Salt -Length 16
            $key = Generate-EncryptionKey -Password $Password -Salt $salt -Length ($algorithmImpl.KeySize / 8)
            $algorithmImpl.Key = $key
            
            Write-Verbose "Salt = $([Convert]::ToBase64String($salt)); Key = $([Convert]::ToBase64String($algorithmImpl.Key)); IV = $([Convert]::ToBase64String($algorithmImpl.IV))"

            $encryptor = $algorithmImpl.CreateEncryptor($algorithmImpl.Key, $algorithmImpl.IV)
            try 
            {
                $originalFile = New-Object System.IO.FileStream $Source,Open,Read,Read
                try 
                {
                    $encryptedStream = New-Object System.IO.FileStream $Destination,CreateNew,Write,None
                    try 
                    {
                        $encryptedStream.Write($salt, 0, $salt.Length)
                        $encryptedStream.Write($algorithmImpl.IV, 0, $algorithmImpl.IV.Length)
                        $encryptedStream.Flush()

                        $cryptoStream = New-Object System.Security.Cryptography.CryptoStream $encryptedStream,$encryptor,Write
                        try
                        {
                            $buffer = New-Object System.Byte[] 1024
                            $bytesRead = $originalFile.Read($buffer, 0, $buffer.Length)
                            while ($bytesRead -gt 0) 
                            {
                                $cryptoStream.Write($buffer, 0, $bytesRead)
                                $bytesRead = $originalFile.Read($buffer, 0, $buffer.Length)
                            }

                            $cryptoStream.FlushFinalBlock()
                        }
                        finally
                        {
                            $cryptoStream.Dispose()
                        }
                    }
                    finally
                    {
                        $encryptedStream.Dispose()
                    }
                }
                finally
                {
                    $originalFile.Dispose()
                }
            }
            finally
            {
                $encryptor.Dispose()
            }
        }
        finally
        {
            $algorithmImpl.Dispose()
        }
    }
    End
    {
        if ($CurrentEnvironmentCurrentDirectory)
        {
            [System.Environment]::CurrentDirectory = $CurrentEnvironmentCurrentDirectory
        }
    }
}

function Decrypt-File
{
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSUseApprovedVerbs', '')]
    [Diagnostics.CodeAnalysis.SuppressMessageAttribute('PSAvoidUsingPlainTextForPassword', 'Password')]
    Param
    (
        [Parameter(Mandatory = $true, Position = 0)]
        [String]
        $Algorithm,
        
        [Parameter(Mandatory = $true, Position = 1)]
        [String]
        $Source,

        [Parameter(Mandatory = $true, Position = 2)]
        [String]
        $Destination,

        [Parameter(Mandatory = $true, Position = 3)]
        [String]
        $Password
    )
    Begin
    {
        $CurrentEnvironmentCurrentDirectory = [System.Environment]::CurrentDirectory
        [System.Environment]::CurrentDirectory = Get-Location
    }
    Process
    {
        $algorithmImpl = Create-SymmetricAlgorithm -Name $Algorithm
        try 
        {
            $encryptedStream = New-Object System.IO.FileStream $Source,Open,Read,Read
            try 
            {
                $salt = New-Object System.Byte[] 16
                if ($encryptedStream.Read($salt, 0, $salt.Length) -ne $salt.Length) { throw "Unable to read Salt" }

                $iv = New-Object System.Byte[] ($algorithmImpl.BlockSize / 8)
                if ($encryptedStream.Read($iv, 0, $iv.Length) -ne $iv.Length) { throw "Unable to read IV" }

                $key = Generate-EncryptionKey -Password $Password -Salt $salt -Length ($algorithmImpl.KeySize / 8)
                $algorithmImpl.Key = $key
                $algorithmImpl.IV = $iv

                Write-Verbose "Salt = $([Convert]::ToBase64String($salt)); Key = $([Convert]::ToBase64String($algorithmImpl.Key)); IV = $([Convert]::ToBase64String($algorithmImpl.IV))"

                $originalFile = New-Object System.IO.FileStream $Destination,CreateNew,Write,None
                try 
                {
                    $decryptor = $algorithmImpl.CreateDecryptor($algorithmImpl.Key, $algorithmImpl.IV)
                    try 
                    {
                        $cryptoStream = New-Object System.Security.Cryptography.CryptoStream $encryptedStream,$decryptor,Read
                        try
                        {
                            $buffer = New-Object System.Byte[] 1024
                            $bytesRead = $cryptoStream.Read($buffer, 0, $buffer.Length)
                            while ($bytesRead -gt 0) 
                            {
                                $originalFile.Write($buffer, 0, $bytesRead)
                                $bytesRead = $cryptoStream.Read($buffer, 0, $buffer.Length)
                            }
                        }
                        finally
                        {
                            $cryptoStream.Dispose()
                        }
                    }
                    finally 
                    {
                        $decryptor.Dispose()
                    }
                }
                finally
                {
                    $originalFile.Dispose()
                }
            }
            finally
            {
                $encryptedStream.Dispose()
            }
        }
        finally
        {
            $algorithmImpl.Dispose()
        }
    }
    End
    {
        if ($CurrentEnvironmentCurrentDirectory)
        {
            [System.Environment]::CurrentDirectory = $CurrentEnvironmentCurrentDirectory
        }
    }
}

Export-ModuleMember Encrypt-StringToBase64
Export-ModuleMember Decrypt-Base64ToString
Export-ModuleMember Encrypt-File
Export-ModuleMember Decrypt-File