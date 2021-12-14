ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.22-cbl-mariner1.0

# Install ASP.NET Core
RUN aspnetcore_version=3.1.22 \
    && curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-x64.rpm \
    && aspnetcore_sha512='ff863a174a72ebb5e286eccfb1eb9b6bf6cc5ea543c7da28120de2a196f292fcdff9cd58502642a5de692b89f99000e3980969b1b3e7a6a3f917527eb606a9af' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
