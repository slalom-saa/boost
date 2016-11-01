<#
.SYNOPSIS
    Configures the local development environment to work with the project.
.DESCRIPTION
	This script first adds chocolatey, and then installs tools and utilities that are required and/or
	helpful.  If scripts are not enabled, you must first run: "Set-ExecutionPolicy -ExecutionPolicy Unrestricted -Force"
#>

iex ((new-object net.webclient).DownloadString('https://chocolatey.org/install.ps1'))
choco upgrade powershell -y

# Can run during other installs
choco install 7zip.install -y
choco install git.install -y
choco install smtp4dev -y
choco install sublimetext3 -y
choco install fiddler4 -y

# Run individually
choco install windowsazurepowershell -y
choco install sourcetree -y
choco install nodejs.install -y
choco install azurestorageexplorer -y
choco install mssqlserver2014express -y
choco install webessentials2015 -y

# requires license
choco install ncrunch-vs2015 -y --force
choco install resharper -y
choco install reflector -y

# Optional
# choco install hipchat -y
# choco install slack -y