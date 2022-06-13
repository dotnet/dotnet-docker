ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.6-cbl-mariner2.0-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.6

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='a3bd7ce99ffb9b87766c49fcf28d802f4072af1d55f1d53ef4043a1a0b038a0fc8046669bbd82f64fb37e4c73703fa8f54a460caaa473d952baf941d23341c90' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
