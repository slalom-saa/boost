<#
.SYNOPSIS
    Clears the Visual Studio Experimental Instance Cache
.DESCRIPTION
	The Visual Studio Experimental Instance runs when the Package is run.  This Instance is isolated from the normal
	environment to prevent corruption.  This script deletes the cache files so that a clean install can be tested.
#>

IF(Test-Path "$env:userprofile\AppData\Local\Microsoft\VisualStudio\14.0Exp")
{
    Remove-Item "$env:userprofile\AppData\Local\Microsoft\VisualStudio\14.0Exp" -Recurse -Force
	Write-Host "Deleted local 14 experimental cache." -ForegroundColor Green
}
ELSE
{
	Write-Host "Local 14 experimental cache has already been deleted." -ForegroundColor Green
}
IF(Test-Path "$env:userprofile\AppData\Local\Microsoft\VisualStudio\Exp")
{
    Remove-Item "$env:userprofile\AppData\Local\Microsoft\VisualStudio\Exp" -Recurse -Force
	Write-Host "Deleted local experimental cache." -ForegroundColor Green
}
ELSE
{
	Write-Host "Local 14 cache has already been deleted." -ForegroundColor Green
}
IF (Test-Path "$env:userprofile\AppData\Roaming\Microsoft\VisualStudio\14.0Exp")
{
    Remove-Item "$env:userprofile\AppData\Roaming\Microsoft\VisualStudio\14.0Exp" -Recurse -Force
	Write-Host "Deleted roaming 14 experimental cache." -ForegroundColor Green
}
ELSE
{
	Write-Host "Roaming 14 experimental cache has already been deleted." -ForegroundColor Green
}

