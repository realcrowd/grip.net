rmdir /s /q package

if not exist package mkdir package
if not exist package\lib mkdir package\lib
if not exist package\lib\net45 mkdir package\lib\net45

copy src\RealCrowd.PublishControl\bin\Release\RealCrowd.PublishControl.dll package\lib\net45
copy src\RealCrowd.Grip\bin\Release\RealCrowd.Grip.dll package\lib\net45

nuget pack RealCrowd.Grip.nuspec -BasePath package
