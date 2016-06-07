REM Interim script to assist in building Windows images until Docker Hub auto builds support Windows

Set DOTNET_SDK_VERSION=1.0.0-preview2
Set DOTNET_VERSION=1.0.0-rc3

IF NOT DEFINED REPO_OWNER (
    set REPO_OWNER=microsoft
)

IF DEFINED NANO_SERVER (
    set WINDOWS_FLAVOR=nanoserver
) ELSE (
    set WINDOWS_FLAVOR=windowsservercore
)

pushd %DOTNET_SDK_VERSION%\%WINDOWS_FLAVOR%\x64
docker build -t %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x64 . || goto :error
docker tag %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x64 %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-%WINDOWS_FLAVOR%-x64 || goto :error
popd

pushd %DOTNET_SDK_VERSION%\%WINDOWS_FLAVOR%\x64\onbuild
docker build -t %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x64-onbuild . || goto :error
docker tag %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x64-onbuild %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-%WINDOWS_FLAVOR%-x64-onbuild || goto :error
popd

pushd %DOTNET_VERSION%\%WINDOWS_FLAVOR%\x64\core
docker build -t %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x64-core . || goto :error
docker tag %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x64-core %REPO_OWNER%/dotnet:%DOTNET_VERSION%-%WINDOWS_FLAVOR%-x64-core || goto :error
popd

pushd %DOTNET_SDK_VERSION%\%WINDOWS_FLAVOR%\x86
docker build -t %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x86 . || goto :error
docker tag %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x86 %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-%WINDOWS_FLAVOR%-x86 || goto :error
popd

pushd %DOTNET_SDK_VERSION%\%WINDOWS_FLAVOR%\x86\onbuild
docker build -t %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x86-onbuild . || goto :error
docker tag %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x86-onbuild %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-%WINDOWS_FLAVOR%-x86-onbuild || goto :error
popd

pushd %DOTNET_VERSION%\%WINDOWS_FLAVOR%\x86\core
docker build -t %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x86-core . || goto :error
docker tag %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x86-core %REPO_OWNER%/dotnet:%DOTNET_VERSION%-%WINDOWS_FLAVOR%-x86-core || goto :error
popd

goto :EOF

:error
popd
exit /B 1
