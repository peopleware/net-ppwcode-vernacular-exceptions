###############################################################################
###  PSAKE build script                                                     ###
###############################################################################
###  Copyright 2015 by PeopleWare n.v.                                      ###
###############################################################################
###  Authors: Ruben Vandeginste, Danny Van den Wouwer                       ###
###############################################################################
###                                                                         ###
###  A psake build file that configures                                     ###
###     - build and clean the solution                                      ###
###     - restore nuget packages                                            ###
###     - create and publish nuget packages                                 ###
###     - run unit tests                                                    ###
###     - generate documentation                                            ###
###                                                                         ###
###############################################################################


#region CONFIGURATION

###############################################################################
### CONFIGURATION                                                           ###
###############################################################################

###############################################################################
# Use build tools from .NET framework 4.7
#
Framework '4.7'

###############################################################################
# Properties that are used by this psake
# script; these can be overridden when
# invoking this script
#
Properties {
    # buildconfig: the configuration used for building
    # note that this also determines the deployment configuration (SlowCheetah)
    $buildconfig = 'Debug'
    $buildPlatformTarget = 'AnyCpu'

    # chatter: number to indicate verbosity during the execution of tasks
    #   0 - no output
    #   1 - minimal output, tasks executed
    #   2 - extra output, steps within tasks
    #   3 - even more output
    $chatter = 1
    # chattercolor: foreground color to use for task output
    $chattercolor = 'Green'
    # used nunit version
    $nunitVersion = '3.8.0'
    $nUnitFramework = $null
}

#endregion


#region PRIVATE HELPERS

###############################################################################
### PRIVATE HELPERS                                                         ###
###############################################################################

###############################################################################
# Helper for printing out documentation
#
function PropertyDocumentation() {
    Write-Host 'Properties'
    Write-Host '----------'
    Write-Host "`$chatter                      The level of verbosity during task execution, defaults to 1."
    Write-Host  '                                0 - No output'
    Write-Host  '                                1 - Minimal output, tasks executed'
    Write-Host  '                                2 - Extra output, steps within tasks'
    Write-Host  '                                3 - Even more output, debug execution'
    Write-Host "`$chattercolor                 Foreground color to use for task output., defaults to Green"
    Write-Host "`$buildconfig                  Build configuration (Debug, Release), defaults to Debug."
    Write-Host "`$buildPlatformTarget          Build Target Platform (AnyCpu, x86, x64), defaults to AnyCpu."
    Write-Host "`$nunitVersion                 Version of nunit-runner, defaults to 3.8.0"
    Write-Host "`$nUnitFramework               RuntimeFramework for nunit-runner, defaults to $null, If not specified, tests will run under the framework they are compiled with"
}

###############################################################################
# Helper for chatter
#
function Chatter {
    param
    (
        [string]
        $msg = '.',

        [int]
        $level = 3
    )

    if ($level -le $chatter) {
        Write-Host $msg -ForegroundColor $chattercolor
    }
}

###############################################################################
# Helper for to see if we run with Administrator privileges
#
function Test-Administrator
{
    $user = [Security.Principal.WindowsIdentity]::GetCurrent();
    (New-Object Security.Principal.WindowsPrincipal $user).IsInRole([Security.Principal.WindowsBuiltinRole]::Administrator)
}

###############################################################################
# Helper to determine our msbuild parameters
#
function Get-DefaultMsBuildParameters
{
    Param (
        [Parameter(Mandatory = $true)]
        [string]$solution,

        [Parameter(Mandatory = $true)]
        [Array]$tasks,

        [Parameter(Mandatory = $true)]
        [string]$configuration,

        [Parameter(Mandatory = $false)]
        [string[]]$additionalParameters
    )

    $concatedTasks = ($tasks -join ';')
    $params = @(
        """$solution"""
        "/t:$concatedTasks"
        "/m"
        "/nr:false"
        "/v:quiet"
        "/nologo"
        "/p:Configuration=$configuration"
    )

    if ($additionalParameters) {
        foreach($param in $additionalParameters) {
            $params += $param
        }
    }

    $params
}

###############################################################################
# Helper to invoke msbuild
#
function Invoke-MsBuild
{
    Param (
        [Parameter(Mandatory = $true)]
        [string]$solution,

        [Parameter(Mandatory = $true)]
        [Array]$tasks,

        [Parameter(Mandatory = $true)]
        [string]$configuration,

        [Parameter(Mandatory = $false)]
        [string[]]$additionalParameters
    )

    $msbuildParams = Get-DefaultMsBuildParameters -solution $solution -tasks $tasks -configuration $configuration -additionalParameters $additionalParameters
    Chatter "msbuild $msbuildParams" 3
    Exec { msbuild $msbuildParams }
}

#endregion

#region TASKS

###############################################################################
### TASKS                                                                   ###
###############################################################################

###############################################################################
# Default task, executed when no task is
# given explicitly
#
Task Default -depends ?

###############################################################################
# Help
#
Task ? -description 'Show help.' {
    Write-Host
    PropertyDocumentation
    WriteDocumentation
}

###############################################################################
# Helper to chat our properties
#
Task ShowProperties -description 'Show running properties' {
    Chatter "Properties used:" 1
    Chatter "  buildconfig=$buildconfig" 1
    Chatter "  buildPlatformTarget=$buildPlatformTarget" 1
    Chatter "  nunitVersion=$nunitVersion" 1
    Chatter "  nUnitFramework=$nUnitFramework" 1
}

###############################################################################
# Clean ReSharper cache folder
#
Task ReSharperClean -description 'Clean ReSharper cache folders.' -depends ShowProperties {

    Push-Location
    try
    {
        # clean up cache folder
        $path = Join-Path "src" "_ReSharper.Caches"
        if (Test-Path $path) {
            Remove-Item -Path $path -Recurse -Force
        }
    }
    finally
    {
        Pop-Location
    }
}

###############################################################################
# Restore packages
#
Task PackageRestore -description 'Restore NuGet package dependencies.' -depends ShowProperties {

    Push-Location
    try
    {
        Set-Location 'src'
        $solution = Get-Item '*.sln' | Select-Object -First 1

        # traditional nuget command-line restore
        # note, nuget commandline transparantly calls msbuild "restore" for new type csproj files
        $nugetRestoreParams = @(
            "$solution"
            "-NonInteractive"
            "-Verbosity quiet"
        )
        Chatter "nuget restore $nugetRestoreParams" 3
        Exec { Invoke-Expression "nuget restore $nugetRestoreParams" }
    }
    finally
    {
        Pop-Location
    }
}

###############################################################################
# Restore
#
# Depends on PackageRestore to restore packages, and Clean to put the solution
# in a clean state so VS2017 is willing to open it without complaints.
#
Task Restore -description 'Restore solution.' -depends PackageRestore,Clean {
}

###############################################################################
# Build the solution
#
# Depends on Clean to ensure a clean build, not using any left-over compile artifacts.
#
Task Build -description 'Build the solution.' -depends Clean {

    Push-Location
    try
    {
        Set-Location 'src'
        $solution = Get-Item '*.sln' | Select-Object -First 1
        Invoke-MsBuild -solution $solution.Name -tasks @('Restore', 'Build') -configuration $buildconfig
    }
    finally
    {
        Pop-Location
    }
}

###############################################################################
# Create a NuGet package
#
# Depends on Clean to ensure a clean build, not using any left-over compile artifacts.
#
Task Pack -description 'Create a nuget-package.' -depends Clean {

    Push-Location
    try
    {
        Set-Location 'src'
        $solution = Get-Item '*.sln' | Select-Object -First 1
        Invoke-MsBuild -solution $solution.Name -tasks @('Restore','Pack') -configuration $buildconfig
    }
    finally
    {
        Pop-Location
    }
}

###############################################################################
# Clean build artifacts and temporary files
#
Task Clean -description 'Clean build output.' -depends ShowProperties {

    Push-Location
    try
    {
        Chatter 'Cleaning solution' 2
        Set-Location 'src'
        $solution = Get-Item '*.sln' | Select-Object -First 1

        # msbuild clean
        Invoke-MsBuild -solution $solution.Name -tasks @('Clean') -configuration $buildconfig
        Set-Location '..'

        # clean up scratch folder
        Chatter 'Cleaning scratch' 2
        $path = "scratch"
        if (Test-Path $path) {
            Remove-Item -Path $path -Recurse -Force
        }

        # clean up bin/obj folders
        Chatter 'Cleaning bin/obj' 2
        Remove-Item -Path (Join-Path (Join-Path "src" "*") "bin"),(Join-Path (Join-Path "src" "*") "obj") -Recurse -Force
    }
    finally
    {
        Pop-Location
    }
}

###############################################################################
# Full clean
#
Task FullClean -description 'Clean build output, generated packages and NuGet packages folder.' -depends Clean

###############################################################################
# Full build
#
Task FullBuild -description 'Do a full build starting from a clean solution.' -depends Restore,Build

###############################################################################
# Run unit tests
#
Task Test -description 'Run the unit tests using NUnit console test runner.' -depends FullBuild {

    Push-Location
    try
    {
        $nunitApplicationName = 'nunit3-console.exe'
        $nunitNugetPackageName = 'NUnit.ConsoleRunner'
        $nunitNugetVersionedPackageName = "$nunitNugetPackageName.$($nunitVersion)"
        $packageNUnitPath = `
            Join-Path -Path 'src' -ChildPath 'packages' | `
            Join-Path -ChildPath $nunitNugetVersionedPackageName

        # Can we find nunit-runner in our traditional packages directory?
        $runner = $null
        $runnerPath = `
            Join-Path -Path $packageNUnitPath -ChildPath 'tools' | `
            Join-Path -ChildPath $nunitApplicationName
        if (Test-Path $runnerPath) {
            $runner = Get-Item -Path $runnerPath
        }

        # If we didn't found it, install nunit-runner in our scratch directory
        if ($null -eq $runner) {
            $scratchNUnitPath = Join-Path -Path '.\scratch' -ChildPath 'nunit'

            $nugetRestoreParams = @(
                "$nunitNugetPackageName"
                "-Version '$($nunitVersion)'"
                "-OutputDirectory ""$scratchNUnitPath"""
                "-NonInteractive"
                "-Verbosity quiet"
            )
            Chatter "nuget install $nugetRestoreParams" 3
            Exec { Invoke-Expression "nuget install $nugetRestoreParams" }

            $runnerPath = `
                Join-Path -Path $scratchNUnitPath -ChildPath $nunitNugetVersionedPackageName | `
                Join-Path -ChildPath 'tools' | `
                Join-Path -ChildPath $nunitApplicationName
            if (Test-Path $runnerPath) {
                $runner = Get-Item $runnerPath
            }
        }

        # stop if we don't have a nunit-runner
        if ($null -eq $runner)
        {
            throw 'Cannot find or fetch nunit runner.'
        }

        Chatter "Found nunit runner: $runner" 3

        # find unit test dlls
        Chatter 'Searching test projects ...' 3
        Get-ChildItem -Path (Join-Path -Path 'scratch' -ChildPath 'bin') -Filter *Tests.dll -Recurse | ForEach-Object {
            # generate folder for test output
            Chatter "  Found test project: $_.Name" 3

            $nunitFolder = New-Item -ItemType Directory -Path (Join-Path -Path "scratch" -ChildPath "nunit") -Force
            $work = Join-Path $nunitFolder $testdll.BaseName

            # intialise runner args
            $testrunnerargs = @(
                "--work:$work"
                '--result:nunit-test-results.xml'
                '--out:nunit-stdout.txt'
                '--err:nunit-stderr.txt'
                '--labels:all'
                )

            if ($null -ne $nUnitFramework) {
                $testrunnerargs += "--framework:$nUnitFramework"
            }

            # execute runner
            Chatter "$($runner.FullName) $_.FullName $testrunnerargs" 3
            Exec { & "$($runner.FullName)" $_.FullName $testrunnerargs }
        }
    }
    finally
    {
        Pop-Location
    }
}
#endregion
