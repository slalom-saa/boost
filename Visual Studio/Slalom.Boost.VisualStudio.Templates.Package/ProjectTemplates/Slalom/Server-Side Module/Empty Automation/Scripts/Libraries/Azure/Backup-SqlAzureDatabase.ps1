<#
.SYNOPSIS
    Back up a SqlAzure Database to a given storage account in a given subscription.
.DESCRIPTION
    First this workflow requires SqlAuthentication and open connection to the SqlAzure server, so please add the execution host into the server's 
    firewall rule. Otherwise, backup will fail.
    Second, if the given blob already exists in the given storage account, the workflow will not overwrite and will fail.
    The workflow places the exported SqlAzure Database file as given blob name in given storage container of given storage account.


.EXAMPLE
    import-module .\Backup-SqlAzureDatabase.psm1
    Backup-SqlAzureDatabase -SubscriptionId "11111111-aaaa-bbbb-cccc-222222222222" `
        -SqlAzureServerName MyServerName `
        -SqlAzureDatabaseName MyDBName -SqlAzureUserName myusername -SqlAzurePassword mypassword -StorageAccountName MyStorage -StroageContainerName MyContainer`
        -StorageBlobName myblob
#>

workflow Backup-SqlAzureDatabase
{
    param(
       
                [parameter(Mandatory=$true)]
                [String]$SubscriptionId,                
	
                [Parameter(Mandatory = $true)] 
                [String]$SqlAzureServerName,

                [Parameter(Mandatory = $true)] 
                [String]$SqlAzureDatabaseName,

                [Parameter(Mandatory = $true)] 
                [String]$SqlAzureUserName,

                [Parameter(Mandatory = $true)] 
                [String]$SqlAzurePassword,

                [Parameter(Mandatory = $true)]
                [String]$StorageAccountName,

                [Parameter(Mandatory = $true)]
                [String]$StorageContainerName,

                [Parameter(Mandatory = $true)]
                [String]$StorageBlobName
    )

    # Check if Windows Azure Powershell is avaiable
    if ((Get-Module -ListAvailable Azure) -eq $null)
    {
        throw "Windows Azure Powershell not found! Please install from http://www.windowsazure.com/en-us/downloads/#cmd-line-tools"
    }

    $Start = [System.DateTime]::Now
    "Starting: " + $Start.ToString("HH:mm:ss.ffffzzz")

    $SqlCredential = new-object System.Management.Automation.PSCredential($SqlAzureUserName, ($SqlAzurePassword | ConvertTo-SecureString -asPlainText -Force))
    Write-Debug $SqlCredential
    
    inlinescript
    {
        Select-Azuresubscription -SubscriptionId "$using:SubscriptionId"     
     
        $StoragePrimaryKey=(Get-AzureStorageKey -StorageAccountName $using:StorageAccountName).Primary
        Write-Output ("Storage Account primary key : {0} " -f $StoragePrimaryKey)

        $Storagectx = New-AzureStorageContext -StorageAccountName $using:StorageAccountName -StorageAccountKey $StoragePrimarykey
        Write-Debug $Storagectx

        
        $SqlDBCtx=New-AzureSqlDatabaseServerContext -ServerName $using:SqlAzureServerName -Credential $using:SqlCredential
        Write-Debug $SqlDBCtx

        if (($StorageCtx -ne $null) -And ($SqlDBCtx -ne $null))
        {
            $ExportOP = Start-AzureSqlDatabaseExport -SqlConnectionContext $SqlDBCtx -StorageContext $Storagectx -StorageContainerName $using:StorageContainerName -DatabaseName $using:SqlAzureDatabaseName -blobname $using:StorageBlobName
            $ExportStatus = Get-AzureSqlDatabaseImportExportStatus -username $using:SqlAzureUserName -password $using:SqlAzurePassword -servername $using:SqlAzureServerName -RequestId $ExportOP.RequestGuid
  
            while ($ExportStatus.Status -ne "Completed" -And $ExportStatus.Status -ne "Failed")
            {
               Write-OutPut ("Backing up Sql Azure Database {0} on server {1} : status {2} " -f $using:SqlAzureDatabaseName, $using:SqlAzureServerName, $ExportStatus.Status)
               Start-Sleep 20
               $ExportStatus = Get-AzureSqlDatabaseImportExportStatus -username $using:SqlAzureUserName -password $using:SqlAzurePassword -servername $using:SqlAzureServerName -RequestId $ExportOP.RequestGuid
            }

            Write-OutPut ("Backing up Sql Azure Database {0} on server {1}: status {2} " -f $using:SqlAzureDatabaseName, $using:SqlAzureServerName, $ExportStatus.Status)        
            if ($ExportStatus.Status -eq "Failed")
            {
                Write-Err -message $ExportStatus.ErrorMessage
            }
        }
    }

    $Finish = [System.DateTime]::Now
    $TotalUsed = $Finish.Subtract($Start).TotalSeconds
   
    Write-Output ("Backed up Sql Azure Database {0} on server {1} in subscription {2} in {3} seconds." -f $SqlAzureDatabaseName, $SqlAzureServerName, $SubscriptionName, $TotalUsed)
    "Finished " + $Finish.ToString("HH:mm:ss.ffffzzz")
} 

