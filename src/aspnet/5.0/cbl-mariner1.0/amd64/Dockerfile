ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.13-cbl-mariner1.0-amd64

ENV ASPNET_VERSION=5.0.13

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='1b5e9a990459ec69341171021e8cbf20e875a5d6b98204bdd6baf96e956ff3dfe350269eb9086734dd07465372d56dadb779c0e2db4566d67345084f5572c899' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
