ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-buster-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.14 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='23ac00b88da24072d39c1d3ebfcea60fba9f0891961c9f9ac1f49f686f1089b9b7e0227440eee071bba98d887492b9dd6dc2c5e9ebdccd536d705425dc26efae' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
