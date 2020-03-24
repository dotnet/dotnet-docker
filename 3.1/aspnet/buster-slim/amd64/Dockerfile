ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim

# Install ASP.NET Core
RUN aspnetcore_version=3.1.3 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='d1d7e9ec0d3d5ab2b3fd932bef5e34e6d247b7073b41c09e74cac1c3e46bca7629eff07c458ac6c87663ea5c985c984943a9806972cf270107519c095c9dc91c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
