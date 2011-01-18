Dependencies
------------

Outside code on which this solution is dependent is gathered in binary
form in this "lib" directory. Projects in the solution refer to
files in this directory as references.
These dependencies are needed for compilation and build, and for running
the code. When you use (parts of) this solutions in other products,
you need the artifacts this solution depends on, recursively, also in
the other product.

When possible, a copy of the required files is kept in this directory
in the repository, so that developers can get started easily. However,
for some dependencies, license issues prohibit us to distribute the
dependencies ourselfs. The developer needs to retrieve them from the
original source himself. In this case, this document describes how to
get the required files.


Microsoft Contracts
-------------------

Version: Release 1.4.31130.0 (November 30, 2010)
(see <http://research.microsoft.com/en-us/projects/contracts/releasenotes.aspx>)

This solution uses Microsoft Contracts. It is required both at build
time, and at runtime.

To get the required DLL:
* Download the distribution from
  <http://msdn.microsoft.com/en-us/devlabs/dd491992>.
  We use the "Standard Edition".
  The downloaded file is "Contracts.devlab9std.msi".
* Run the installer (this also installs extra components in Visual Studio)
* Copy the file "Microsoft.Contracts.dll" from
  "C:\Program Files\Microsoft\Contracts\PublicAssemblies\v3.5\"
  to this directory.
