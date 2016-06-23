REM Interim script to assist in building Windows images until Docker Hub auto builds support Windows

Set DOTNET_SDK_VERSION=1.0.0-preview2
Set DOTNET_VERSION=1.0.0

IF NOT DEFINED REPO_OWNER (
    set REPO_OWNER=microsoft
)

IF DEFINED NANO_SERVER (
    pushd %DOTNET_SDK_VERSION%\nanoserver
    docker build -t %REPO_OWNER%/dotnet:nanoserver . || goto :error
    docker tag %REPO_OWNER%/dotnet:nanoserver %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-nanoserver-sdk || goto :error
    popd

    pushd %DOTNET_SDK_VERSION%\nanoserver\onbuild
    docker build -t %REPO_OWNER%/dotnet:nanoserver-onbuild . || goto :error
    docker tag %REPO_OWNER%/dotnet:nanoserver-onbuild %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-nanoserver-onbuild || goto :error
    popd

    pushd %DOTNET_VERSION%\nanoserver\core
    docker build -t %REPO_OWNER%/dotnet:nanoserver-core . || goto :error
    docker tag %REPO_OWNER%/dotnet:nanoserver-core %REPO_OWNER%/dotnet:%DOTNET_VERSION%-nanoserver-core || goto :error
    popd
) ELSE (
    pushd %DOTNET_SDK_VERSION%\windowsservercore
    docker build -t %REPO_OWNER%/dotnet:windowsservercore . || goto :error
    docker tag %REPO_OWNER%/dotnet:windowsservercore %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-windowsservercore-sdk || goto :error
    popd

    pushd %DOTNET_SDK_VERSION%\windowsservercore\onbuild
    docker build -t %REPO_OWNER%/dotnet:windowsservercore-onbuild . || goto :error
    docker tag %REPO_OWNER%/dotnet:windowsservercore-onbuild %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-windowsservercore-onbuild || goto :error
    popd

    pushd %DOTNET_VERSION%\windowsservercore\core
    docker build -t %REPO_OWNER%/dotnet:windowsservercore-core . || goto :error
    docker tag %REPO_OWNER%/dotnet:windowsservercore-core %REPO_OWNER%/dotnet:%DOTNET_VERSION%-windowsservercore-core || goto :error
    popd

goto :EOF

:error
popd
exit /B 1
