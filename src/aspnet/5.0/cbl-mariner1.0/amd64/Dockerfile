ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0.14-cbl-mariner1.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.14

# Install ASP.NET Core
RUN curl -fSL --output aspnetcore.rpm https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-x64.rpm \
    && aspnetcore_sha512='df9697d3e0d7d40d222165511f4a3a8169f30ae061189386d81485375fea630898abb662268c9ba7fdfb6e97881fbfaa3ebc1ef79de2617b003d561f5da07a54' \
    && echo "$aspnetcore_sha512  aspnetcore.rpm" | sha512sum -c - \
    && rpm --install aspnetcore.rpm \
    && rm aspnetcore.rpm
