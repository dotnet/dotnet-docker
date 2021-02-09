ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-bionic-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.12 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='656f95054ca2a40f12f993a7b2d0f734ab7952f52ced8bb52c4c2074a68b93f82fcbc191a215587306f0a7b82e7aec0bd1473de8bf2e8842fabab4d0a8809ead' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
