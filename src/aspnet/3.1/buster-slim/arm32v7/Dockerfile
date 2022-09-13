ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.29-buster-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.29 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='9b14938c7d456a86782eb63c527e74f6543b919e8ec4e86e3b50c9356d52da2a1218a3467a1f18597fe1579fda53d3aae2ab37a810ffe51abc884d9fc1c4496a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
