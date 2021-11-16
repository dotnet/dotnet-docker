ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:7.0.0-preview.1-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-preview.1.22109.13

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='0888a6c704ab588d7449b88df8551ce3de81e3d10c65117ed9f788b87677bdcb6a174daab08ebb02c96cbaf96b2704baeb6a25c03985a674f78c1eaf1c6db9ff' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
