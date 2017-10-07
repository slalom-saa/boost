param (
	$Url = "https://www.nuget.org",
	$NugetPath = "NuGet.exe",
    $ApiKey = '4fdba183-fa91-4650-8dae-760bf2c22337'
)

$location = $PSScriptRoot

cd $location
..\Slalom.Boost\.nuget\NuGetPackage.ps1

cd $location
..\Slalom.Boost.DocumentDb\.nuget\NuGetPackage.ps1

cd $location
..\Slalom.Boost.EntityFramework\.nuget\NuGetPackage.ps1

cd $location
..\Slalom.Boost.MongoDB\.nuget\NuGetPackage.ps1

cd $location
..\Slalom.Boost.RabbitMq\.nuget\NuGetPackage.ps1

cd $location
..\Slalom.Boost.WebApi\.nuget\NuGetPackage.ps1


cd C:\NuGet

Get-ChildItem Slalom.*.nupkg | Where-Object { $_.Name.EndsWith(".symbols.nupkg") -eq $false } | ForEach-Object { 
    try {
	# Try to push package
	& $NugetPath push $_.FullName $ApiKey -Source $Url	
}catch
{
}
}