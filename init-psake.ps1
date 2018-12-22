###############################################################################
###  Bootstrap script for PSAKE                                             ###
###############################################################################
###  Copyright 2017 by PeopleWare n.v..                                     ###
###############################################################################
###  Authors: Ruben Vandeginste, Danny Van den Wouwer                       ###
###############################################################################
###                                                                         ###
###  A script to bootstrap the powershell session for psake.                ###
###                                                                         ###
###############################################################################

#region INPUT PARAMATERS

###############################################################################
### INPUT PARAMETERS                                                        ###
###############################################################################

###############################################################################
# input parameters
#   target: Task to execute
param([string]$target = '')

#endregion


#region HELPERS

###############################################################################
### HELPERS                                                                 ###
###############################################################################

###############################################################################
# Stolen from Psake, helper function to execute external commands and respect
# their exit code.
#
function Exec {
    [CmdletBinding()]
    param (
        [Parameter(Position = 0, Mandatory = 1)][scriptblock]$cmd,
        [Parameter(Position = 1, Mandatory = 0)][string]$errorMessage = 'Bad command.',
        [Parameter(Position = 2, Mandatory = 0)][int]$maxRetries = 0,
        [Parameter(Position = 3, Mandatory = 0)][string]$retryTriggerErrorPattern = $null
    )

    $tryCount = 1

    do
    {
        try
        {
            $global:lastexitcode = 0
            & $cmd
            if ($lastexitcode -ne 0)
            {
                throw ('Exec: ' + $errorMessage)
            }
            break
        }
        catch [Exception]
        {
            if ($tryCount -gt $maxRetries)
            {
                throw $_
            }

            if ($retryTriggerErrorPattern -ne $null)
            {
                $isMatch = [regex]::IsMatch($_.Exception.Message, $retryTriggerErrorPattern)

                if ($isMatch -eq $false)
                {
                    throw $_
                }
            }

            Write-Host "Try $tryCount failed, retrying again in 1 second..."

            $tryCount++

            [System.Threading.Thread]::Sleep([System.TimeSpan]::FromSeconds(1))
        }
    }
    while ($true)
}

###############################################################################
# Throw an error if the given command is not available
#
function CheckCommandAvailability {
    param(
        [String]
        $cmd
    )

    if (!$(Get-Command "$cmd" -ErrorAction SilentlyContinue)) {
        throw "'$cmd' not available from the commandline!"
    }
}

#endregion


#region MAIN

###############################################################################
### MAIN                                                                    ###
###############################################################################

###############################################################################
# Main script to bootstrap psake.
#

try
{
    # find module, if not found, try to download it
    $module = $null

    $psakeNugetPackageName = 'psake'
    $psakeVersion = '4.7.0'
    $psakeNugetVersionedPackageName = "$psakeNugetPackageName.$($psakeVersion)"

    # Can we find psake in our traditional packages directory?
    $psakeToolsFolder = `
        Join-Path -Path 'src' -ChildPath 'packages' | `
        Join-Path -ChildPath $psakeNugetVersionedPackageName | `
        Join-Path -ChildPath 'tools' | `
        Join-Path -ChildPath 'psake'
    if (Test-Path $psakeToolsFolder) {
        $modulePath = Join-Path -Path $psakeToolsFolder -ChildPath 'psake.psd1'
        if (Test-Path $modulePath) {
            $module = Get-Item $modulePath
        }
    }

    # If we didn't found it, install psake in our scratch directory
    if ($null -eq $module) {
        $scratchPsakePath = '.\scratch'

        $nugetRestoreParams = @(
            "$psakeNugetPackageName"
            "-Version '$($psakeVersion)'"
            "-OutputDirectory ""$scratchPsakePath"""
            "-NonInteractive"
            "-Verbosity quiet"
        )
        Exec { Invoke-Expression "nuget install $nugetRestoreParams" }

        $psakeToolsFolder = `
            Join-Path -Path 'scratch' -ChildPath $psakeNugetVersionedPackageName | `
            Join-Path -ChildPath 'tools' | `
            Join-Path -ChildPath 'psake'
        if (Test-Path $psakeToolsFolder) {
            $modulePath = Join-Path -Path $psakeToolsFolder -ChildPath 'psake.psd1'
            if (Test-Path $modulePath) {
                $module = Get-Item $modulePath
            }
        }
    }

    if ($null -eq $module)
    {
        throw 'Cannot find or fetch psake module, install psake: choco install psake.'
    }

    # import module, force a reload if already loaded
    Import-Module $module.FullName -Force

    # execute the target, if any given
    if ($target -ne '')
    {
        Invoke-psake $target
    }
}
catch
{
    Write-Host 'Error executing psake.ps1' -ForegroundColor DarkYellow
    Write-Host
    # Re-Throw so that the calling code does not continue.
    throw $_
}

#endregion
