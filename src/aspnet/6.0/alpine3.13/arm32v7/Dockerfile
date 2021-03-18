ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.13-arm32v7

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-preview.3.21167.3

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='dc354dc9178ead81898b4f5b0908b22e1282b42888e0039d7a4c1db6402d5bf5b323b5c8d0b341f4abebe51565890958e5da2c0465524ddf89a408a425b87e93' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
