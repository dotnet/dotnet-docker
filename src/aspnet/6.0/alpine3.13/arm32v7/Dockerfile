ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.13-arm32v7

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-preview.3.21181.6

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='cb1fcdbac1cae18d9bdcc3eb49038168a6cccc69bceb194e4c17dd80f2e6e0500dab1035ab35aecea70e0648f548781832e5440b6b08d3502665c5bf09cf96d7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
