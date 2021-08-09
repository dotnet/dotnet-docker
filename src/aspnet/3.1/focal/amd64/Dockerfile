ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.18-focal

# Install ASP.NET Core
RUN aspnetcore_version=3.1.18 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='be29a7611941d9b20d5d3ece64d271bddf6814968b5103ce3c2342ba24bf0382eed3625713ce89957fa15671403af16ccb588397fc0b27e7f028952213e08db6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
