@echo off

@echo *******************************************
@echo * BUILDING SOLUTION IN RELEASE			*
@echo *******************************************
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release /property:GenerateLibraryLayout=false /p:NoWarn=0618 src\TagLib.Portable\TagLib.Portable.csproj
msbuild /verbosity:quiet /fl /t:Rebuild /p:Configuration=Release /property:GenerateLibraryLayout=false /p:NoWarn=0618 src\TagLib.Silverlight\TagLib.Silverlight.csproj

@echo *******************************************
@echo * COPYING BINARIES FOR NUGET              *
@echo *******************************************
mkdir .\TagLib\lib\
copy src\TagLib.Portable\bin\release\TagLib.Portable.dll .\TagLib\lib\
copy src\TagLib.Silverlight\bin\release\TagLib.Silverlight.dll .\TagLib\lib\

pushd nuget

@echo *******************************************
@echo * BUILDING NUGET PAKCAGE					*
@echo *******************************************
nuget pack TagLib.Portable.nuspec -o .\
nuget pack TagLib.Silverlight.nuspec -o .\

@echo *******************************************
@echo * DONE BUILDING NUGET - 					*
@echo * DON'T FORGET TO PUBLISH					*
@echo *******************************************

popd