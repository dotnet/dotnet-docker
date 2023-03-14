ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.15-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.15

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='36dc96ec3906f67835a5bed0427cdbecafa0b4a3a82781df537c206fb1facb5e339a0e45b5dd6cf43f069d8cfa0882ef725bcd805e5a339593e1b26c139c615d' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
