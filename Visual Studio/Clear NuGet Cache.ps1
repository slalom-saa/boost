<#
.SYNOPSIS
    Clears the Visual Studio Experimental Instance Cache
.DESCRIPTION
	The Visual Studio Experimental Instance runs when the Package is run.  This Instance is isolated from the normal
	environment to prevent corruption.  This script deletes the cache files so that a clean install can be tested.
#>

IF(Test-Path "$env:userprofile\.nuget\packages")
{
    Remove-Item "$env:userprofile\.nuget\packages" -Recurse -Force
	Write-Host "Deleted nuget cache." -ForegroundColor Green
}
ELSE
{
	Write-Host "Nuget cache has already been deleted."
}