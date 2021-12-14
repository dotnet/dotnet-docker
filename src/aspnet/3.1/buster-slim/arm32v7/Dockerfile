ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.22-buster-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.22 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='ff4490c739ba6e86bdd1106d4832759c152a03af3866068cf9e48e9bbabbbd81f2af9842cdcd36610aa221ee894f50952b93f237a2c4e1e1bfa183b5c6119e28' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
