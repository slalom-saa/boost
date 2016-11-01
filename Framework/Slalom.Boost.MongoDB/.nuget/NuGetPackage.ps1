Param (
	$NugetPath = '.\NuGet.exe'
)

$ErrorActionPreference = "Stop"
$global:ExitCode = 1

# Make sure the nuget executable is writable
Set-ItemProperty $NugetPath -Name IsReadOnly -Value $false

Write-Host "Copying bin to lib folder"
Remove-Item .\lib\*
Copy-Item ..\bin\Debug\Slalom.Boost.Mongo* .\lib

Write-Host " "
Write-Host "Creating package..." -ForegroundColor Green

# Create symbols package if any .pdb files are located in the lib folder
If ((Get-ChildItem *.pdb -Path .\lib -Recurse).Count -gt 0) {
	 & $NugetPath pack Package.nuspec -Symbol -Verbosity Detailed
}
Else {
	& $NugetPath pack Package.nuspec -Verbosity Detailed
}
Copy-Item .\*.nupkg '..\..\Nuget Repository'
Remove-Item .\*.nupkg