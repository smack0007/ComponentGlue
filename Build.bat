@ECHO OFF

ECHO Cleaning Build directory...
rmdir /s /q build

ECHO Building ComponentGlue.sln...
msbuild "ComponentGlue.sln" /nologo /verbosity:quiet /target:clean
msbuild "ComponentGlue.sln" /nologo /verbosity:quiet /property:Configuration=Release /property:Platform="Any CPU" /property:OutputPath=.\..\build /property:WarningLevel=2
del build\*.pdb

ECHO Building NuGet package...
copy ComponentGlue.nuspec .\build
nuget pack .\build\ComponentGlue.nuspec