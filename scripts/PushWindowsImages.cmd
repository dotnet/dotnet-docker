REM Interim script to assist in pushing Windows images until Docker Hub auto builds support Windows

Set DOTNET_SDK_VERSION=1.0.0-preview2
Set DOTNET_VERSION=1.0.0

IF NOT DEFINED REPO_OWNER (
    set REPO_OWNER=microsoft
)

IF DEFINED NANO_SERVER (
    docker push %REPO_OWNER%/dotnet:nanoserver
    docker push %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-nanoserver
    docker push %REPO_OWNER%/dotnet:nanoserver-onbuild
    docker push %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-nanoserver-onbuild
    docker push %REPO_OWNER%/dotnet:nanoserver-core
    docker push %REPO_OWNER%/dotnet:%DOTNET_VERSION%-nanoserver-core
) ELSE (
    docker push %REPO_OWNER%/dotnet:windowsservercore-x64
    docker push %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-windowsservercore-x64
    docker push %REPO_OWNER%/dotnet:windowsservercore-x64-onbuild
    docker push %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-windowsservercore-x64-onbuild
    docker push %REPO_OWNER%/dotnet:windowsservercore-x64-core
    docker push %REPO_OWNER%/dotnet:%DOTNET_VERSION%-windowsservercore-x64-core

    docker push %REPO_OWNER%/dotnet:windowsservercore-x86
    docker push %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-windowsservercore-x86
    docker push %REPO_OWNER%/dotnet:windowsservercore-x86-onbuild
    docker push %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-windowsservercore-x86-onbuild
    docker push %REPO_OWNER%/dotnet:windowsservercore-x86-core
    docker push %REPO_OWNER%/dotnet:%DOTNET_VERSION%-windowsservercore-x86-core
)
