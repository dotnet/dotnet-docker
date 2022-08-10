ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.28-focal

# Install ASP.NET Core
RUN aspnetcore_version=3.1.28 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='fd66f9c0d0e9ed57abe5f81650c2ff49c694e05927e5280dbbdee1a9eb4299f0710bdc06ae0af0737c0a0584970b24d3eb952434b45ad8984fe3e37ca95cc1b1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
