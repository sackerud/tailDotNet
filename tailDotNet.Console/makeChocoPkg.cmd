:: Clean the output dirs
dotnet clean

:: Compile
dotnet publish -c Release -r win-x64

:: Create npkg
 choco pack