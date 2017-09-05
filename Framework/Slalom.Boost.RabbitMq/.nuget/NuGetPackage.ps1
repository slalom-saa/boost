Param (
	$NugetPath = 'nuget.exe'
)

cd $PSScriptRoot


Write-Host "Copying bin to lib folder"

Remove-Item .\lib\*
Copy-Item ..\bin\Debug\Slalom.Boost.* .\lib

Write-Host " "
Write-Host "Creating package..." -ForegroundColor Green

# Create symbols package if any .pdb files are located in the lib folder
If ((Get-ChildItem *.pdb -Path .\lib -Recurse).Count -gt 0) {
	 nuget.exe  pack Package.nuspec -Symbol -Verbosity Detailed
}
Else {
	nuget.exe pack Package.nuspec -Verbosity Detailed
}
Copy-Item .\*.nupkg 'c:\nuget'
Remove-Item .\*.nupkg