<#
.SYNOPSIS
    Imports data based on the specified arguments.
.DESCRIPTION
	This script first loads all PowerShell modules and then adds each type.  Scripts within the Commands folder
	should be invoked to import data.  This ensures consistent state and events.
#>
param (
	[String]$BuildConfiguration = "Debug",
	[String]$WorkingDirectory = $PSScriptRoot,
    [String]$ConnectionString = "Data Source=localhost;Initial Catalog=Product;Integrated Security=true",    
    [String]$MongoConnection,
    [String]$Database = "local"
)

$Module = "..\bin\$BuildConfiguration\Product.Project.Module.Automation.dll"

cd $PSScriptRoot

Get-Item "$WorkingDirectory\..\bin\$BuildConfiguration\*.dll" | ForEach-Object {    
    Add-Type -Path $_
}
Add-AppSetting -Name "mongo:Database" -Value $Database
IF ($MongoConnection -ne "") {
    Add-AppSetting -Name "mongo:Connection" -Value $MongoConnection
}

Import-Module .\Configuration\Configuration.psm1


Add-ConnectionString -Name 'Product' -ConnectionString $ConnectionString

# TODO: Add import items