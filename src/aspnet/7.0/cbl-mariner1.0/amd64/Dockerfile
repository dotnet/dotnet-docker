ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.1-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.1.22077.2

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='06ae18ac2be01f0b7159c118e7217dcb9e53aac865ab631778b6e5555529d5627dfa2e67f9b2f5e3ac0c937535f621cf0777c5e4534da5208080f8c98f431fac' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
