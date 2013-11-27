rmdir /s /q package

if not exist package mkdir package
if not exist package\lib mkdir package\lib
if not exist package\lib\net45 mkdir package\lib\net45
if not exist package\src mkdir package\src
if not exist package\src\RealCrowd.PublishControl mkdir package\src\RealCrowd.PublishControl
if not exist package\src\RealCrowd.Grip mkdir package\src\RealCrowd.Grip

copy src\RealCrowd.PublishControl\bin\Release\RealCrowd.PublishControl.dll package\lib\net45
copy src\RealCrowd.PublishControl\bin\Release\RealCrowd.PublishControl.pdb package\lib\net45
copy src\RealCrowd.Grip\bin\Release\RealCrowd.Grip.dll package\lib\net45
copy src\RealCrowd.Grip\bin\Release\RealCrowd.Grip.pdb package\lib\net45
copy src\RealCrowd.PublishControl\*.cs package\src\RealCrowd.PublishControl
copy src\RealCrowd.Grip\*.cs package\src\RealCrowd.Grip

nuget pack RealCrowd.Grip.nuspec -BasePath package -Symbols
