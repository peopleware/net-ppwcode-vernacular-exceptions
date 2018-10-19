# PPWCode.Vernacular.Exceptions

This library is part of the [ppwcode] project and encapsulates the vernacular on exceptions.


## Getting started

### PPWCode.Vernacular.Exceptions III

This is version III of the library, which is designed to work with Microsoft .NET 4.5 and Microsoft .NET Standard 2.0.

The library is available as the [NuGet] package `PPWCode.Vernacular.Exceptions.III` in the [NuGet Gallery].  It can be
installed using the Nuget package manager from inside Visual Studio.


### Earlier versions

#### PPWCode.Vernacular.Exceptions I

Version I of the library is still available in maintenance mode, and is compatible with Microsoft .NET 3.5.

This version is available on the git branch `I/master`. It is currently not available in the [NuGet Gallery].  One can
however build the package oneself and publish it on a local repository.

#### PPWCode.Vernacular.Exceptions II

Version II of the library is still available in maintenance mode, and is compatible with Microsoft .NET 4.5.

Version II is available on the git branch `II/master` and as the [NuGet] package `PPWCode.Vernacular.Exceptions.II` in
the [NuGet Gallery].  It can be installed using the Nuget package manager from inside Visual Studio.


## Build your own

A couple of reasons come to mind as to why you would want to build your own package of
this library. One reason would be that you need a version of the library built
with the debug configuration. Another reason might be that you need features
that are available on master, but that are not yet released.

Building your own package of this library is very easy.  A [psake] build script is
added for this purpose.

Before executing regular [psake] tasks, the environment must first be initialized.
To do this, open a PowerShell prompt, and execute the following in `src\`.

    .\init-psake.ps1

This will initialize your environment. Note that the script assumes that the
[NuGet] commandline client is available on the path.

After the initialization, several [psake] tasks can be executed using the
PowerShell command `Invoke-psake` that is available now. Here are a couple
of examples:

    Invoke-psake
    Invoke-psake ?
    Invoke-psake PackageRestore
    Invoke-psake Package -properties @{ 'configuration'='Debug'; 'repos'=@('nuget'); 'publishrepo' = 'local' }

The last line builds a [NuGet] package using the 'Debug' configuration, and publishes
it to the [NuGet] repository with the name 'local'. The [NuGet] repository 'nuget'
is used to locate the dependent [NuGet] packages.


## Working in Rider on macOS

On macOS, you can use [Rider] to work on this code. Sadly, 1 small change has to be made for [Rider] to be able to 
build. We are looking into the issue.

_**//TODO:** make source link work on Rider_

1. Open `src/III/PPWCode.Vernacular.Exceptions.III.csproj` (right-click on the project in the Solution in the Project 
Explorer, and select **Edit** / **Edit PPWCode.Vernacular.Exceptions.III.csproj** ).
2. Remove or comment out the section that refers to SourceLink:

        <Project>
          …
          <ItemGroup>
            …
            <!--
            <PackageReference Include="Microsoft.SourceLink.GitHub" …>
              …
            </PackageReference>
            -->
            …
          </ItemGroup>
          …
        </Project>

**!!! Never commit this change !!!**

Build should now work.            



## Contributors

See the [GitHub Contributors list].


## [ppwcode]

This package is part of the [ppwcode] project by [PeopleWare n.v.].

More information can be found in the following locations:
* [ppwcode] website
* [GitHub]

Please note that not all information on those sites is up-to-date. We are
currently in the process of moving the code away from the Google code
subversion repositories to git repositories on [GitHub].


### ppwcode .NET

For the .NET libraries, development will be done in the [GitHub] repositories, and all new stable releases will also
be published as packages on the [NuGet Gallery].

We believe in Design By Contract. Preconditions are enforced with `Assert` statements. Postconditions and invariants
are tested in unit tests.

The packages include both the `pdb` and `xml` files, for debugging symbols and documentation respectively. In the future
we might look into using symbol servers.


## License and Copyright

Copyright 2014 - 2018 by [PeopleWare n.v.].

Licensed under the Apache License, Version 2.0 (the "License");
you may not use this file except in compliance with the License.
You may obtain a copy of the License at

http://www.apache.org/licenses/LICENSE-2.0

Unless required by applicable law or agreed to in writing, software
distributed under the License is distributed on an "AS IS" BASIS,
WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
See the License for the specific language governing permissions and
limitations under the License.



[ppwcode]: https://peopleware.atlassian.net/wiki/spaces/PPWCODE/overview
[GitHub]: https://github.com/peopleware

[PeopleWare n.v.]: http://www.peopleware.be/

[NuGet]: https://www.nuget.org/
[NuGet Gallery]: https://www.nuget.org/policies/About

[GitHub]: https://github.com/peopleware

[Microsoft Code Contracts]: http://research.microsoft.com/en-us/projects/contracts/

[psake]: https://github.com/psake/psake

[GitHub Contributors list]: https://github.com/peopleware/net-ppwcode-vernacular-exceptions/graphs/contributors

[Rider]: https://www.jetbrains.com/rider/
