param (
	$Url = "https://www.nuget.org",
	$NugetPath = ".\NuGet.exe",
    $ApiKey = '4fdba183-fa91-4650-8dae-760bf2c22337'
)
cd $PSScriptRoot

Get-ChildItem *.nupkg | Where-Object { $_.Name.EndsWith(".symbols.nupkg") -eq $false } | ForEach-Object { 

	# Try to push package
	& $NugetPath push $_.FullName $ApiKey -Source $Url	
}

IF(Test-Path "$env:userprofile\.nuget\packages")
{
    Remove-Item "$env:userprofile\.nuget\packages" -Recurse -Force
	Write-Host "Deleted nuget cache." -ForegroundColor Green
}
ELSE
{
	Write-Host "Nuget cache has already been deleted."
}