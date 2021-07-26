ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-preview.7-alpine3.13-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.7.21374.2 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='211fbdedfc9844156b9de377fe5293091d64e3158f8f9dccfb749b9b3df1d935ab299a1b771581fa810442898cd4c3a8709213fb3d622bc586958c7227ffdfd5' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
