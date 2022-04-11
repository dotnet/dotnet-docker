ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.16-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.16

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='09dec269eed6775441c3e81946fe197168322c8c9bb22c8882c1f75c858d31cc9c46bcd90fb0ce5b1a496ef5d6ba37fe72162617d7474c14eb125bc293b9bd24' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
