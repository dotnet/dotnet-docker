ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.31-bullseye-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.31 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='77bd03a2f9d3ea15401b7b6004794cad77c43ac972b2fd0817daf73003ce12358009faf60c2058f5a29366514c458b89a0d0f719829996f81f38e8510f70b3e7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
