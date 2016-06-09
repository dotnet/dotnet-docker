REM Interim script to assist in building Windows images until Docker Hub auto builds support Windows

Set DOTNET_SDK_VERSION=1.0.0-preview2
Set DOTNET_VERSION=1.0.0-rc3

IF NOT DEFINED REPO_OWNER (
    set REPO_OWNER=microsoft
)

IF DEFINED NANO_SERVER (
    pushd %DOTNET_SDK_VERSION%\nanoserver
    docker build -t %REPO_OWNER%/dotnet:nanoserver . || goto :error
    docker tag %REPO_OWNER%/dotnet:nanoserver %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-nanoserver || goto :error
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
    pushd %DOTNET_SDK_VERSION%\windowsservercore\x64
    docker build -t %REPO_OWNER%/dotnet:windowsservercore-x64 . || goto :error
    docker tag %REPO_OWNER%/dotnet:windowsservercore-x64 %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-windowsservercore-x64 || goto :error
    popd

    pushd %DOTNET_SDK_VERSION%\windowsservercore\x64\onbuild
    docker build -t %REPO_OWNER%/dotnet:windowsservercore-x64-onbuild . || goto :error
    docker tag %REPO_OWNER%/dotnet:windowsservercore-x64-onbuild %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-windowsservercore-x64-onbuild || goto :error
    popd

    pushd %DOTNET_VERSION%\windowsservercore\x64\core
    docker build -t %REPO_OWNER%/dotnet:windowsservercore-x64-core . || goto :error
    docker tag %REPO_OWNER%/dotnet:windowsservercore-x64-core %REPO_OWNER%/dotnet:%DOTNET_VERSION%-windowsservercore-x64-core || goto :error
    popd

    pushd %DOTNET_SDK_VERSION%\windowsservercore\x86
    docker build -t %REPO_OWNER%/dotnet:windowsservercore-x86 . || goto :error
    docker tag %REPO_OWNER%/dotnet:windowsservercore-x86 %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-windowsservercore-x86 || goto :error
    popd

    pushd %DOTNET_SDK_VERSION%\windowsservercore\x86\onbuild
    docker build -t %REPO_OWNER%/dotnet:windowsservercore-x86-onbuild . || goto :error
    docker tag %REPO_OWNER%/dotnet:windowsservercore-x86-onbuild %REPO_OWNER%/dotnet:%DOTNET_SDK_VERSION%-windowsservercore-x86-onbuild || goto :error
    popd

    pushd %DOTNET_VERSION%\windowsservercore\x86\core
    docker build -t %REPO_OWNER%/dotnet:windowsservercore-x86-core . || goto :error
    docker tag %REPO_OWNER%/dotnet:windowsservercore-x86-core %REPO_OWNER%/dotnet:%DOTNET_VERSION%-windowsservercore-x86-core || goto :error
    popd
)

goto :EOF

:error
popd
exit /B 1
