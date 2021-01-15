ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:6.0-alpine3.12-amd64

# Install ASP.NET Core
ENV ASPNET_VERSION=6.0.0-alpha.1.21064.29

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-$ASPNET_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='615a36f881eef75064ee25e6d2c85e47b10d477c4e905067878019dbfb378b743e3d20b2ce90c60cf5672b2d18becd6366b5151442b1c6bb220521206ba49716' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
