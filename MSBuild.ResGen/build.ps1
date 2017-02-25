& dotnet restore
& dotnet build

mkdir bin\BuildTasks\, bin\BuildTasks\net46, bin\BuildTasks\netcoreapp1.1 -ea 0 > $null
Copy-Item bin\Debug\net46\MSBuild.ResGen.dll ..\..\bin\BuildTasks\net46
Copy-Item bin\Debug\netcoreapp1.1\MSBuild.ResGen.dll ..\..\bin\BuildTasks\netcoreapp1.1
