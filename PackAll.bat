dotnet pack .\Rampastring.XNAUI.csproj -c WindowsDXRelease --version-suffix aa
dotnet pack .\Rampastring.XNAUI.csproj -c UniversalGLRelease --version-suffix aa
dotnet pack .\Rampastring.XNAUI.csproj -c WindowsGLRelease --version-suffix aa
dotnet pack .\Rampastring.XNAUI.csproj -c WindowsXNARelease --version-suffix aa

dotnet pack .\Rampastring.XNAUI.csproj -c WindowsDXDebug --version-suffix aa
dotnet pack .\Rampastring.XNAUI.csproj -c UniversalGLDebug --version-suffix aa
dotnet pack .\Rampastring.XNAUI.csproj -c WindowsGLDebug --version-suffix aa
dotnet pack .\Rampastring.XNAUI.csproj -c WindowsXNADebug --version-suffix aa

mkdir nupkg
pushd bin
FOR /r %%a in (*.*nupkg) do (
  COPY "%%a" "..\nupkg\%%~nxa"
)
popd
