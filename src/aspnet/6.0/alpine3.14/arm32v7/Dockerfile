ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0.0-preview.7-alpine3.14-arm32v7

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-preview.7.21374.2 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

# Install ASP.NET Core
RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='e7c1671a39233309ebe92be9b1a82fe1f7426dde114d518bb51150fdba7415764b35bac44bf53e0b89ea865dd486a0e61ddcf30fc1dba76ab2519f32c1b78629' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
