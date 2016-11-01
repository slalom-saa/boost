<#
.SYNOPSIS
    Runs a job that imports data.
.DESCRIPTION
    Creates a new job and imports treatment data to the local environment.  The new job allows the assemblies to be
	loaded without being locked.
#>

cd $PSScriptRoot

$Job = Start-Job -FilePath '.\Import Data.ps1' -ArgumentList @("Debug", $PSScriptRoot)
WHILE($Job.State -eq 'Running') {
    Receive-Job $Job
    [System.Threading.Thread]::Sleep(500)
}
Receive-Job $Job