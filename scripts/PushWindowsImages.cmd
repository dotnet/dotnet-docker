REM Interim script to assist in pushing Windows images until Docker Hub auto builds support Windows

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

docker push %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x64
docker push %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-%WINDOWS_FLAVOR%-x64
docker push %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x64-onbuild
docker push %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-%WINDOWS_FLAVOR%-x64-onbuild
docker push %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x64-core
docker push %REPO_OWNER%/dotnet:%DOTNET_VERSION%-%WINDOWS_FLAVOR%-x64-core

docker push %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x86
docker push %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-%WINDOWS_FLAVOR%-x86
docker push %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x86-onbuild
docker push %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-%WINDOWS_FLAVOR%-x86-onbuild
docker push %REPO_OWNER%/dotnet:%WINDOWS_FLAVOR%-x86-core
docker push %REPO_OWNER%/dotnet:%DOTNET_VERSION%-%WINDOWS_FLAVOR%-x86-core
