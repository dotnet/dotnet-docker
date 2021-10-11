ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.20-bullseye-slim

# Install ASP.NET Core
RUN aspnetcore_version=3.1.20 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='374e9232aec8a85f1fbcf376e2263f4ee220552ca7902b5cc5f1d1482733e208494ee1394082dd5c067b48a867d69a3a7be648c7670d425e09316c4e519e3ead' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
