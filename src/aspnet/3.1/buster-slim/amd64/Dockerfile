ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.23-buster-slim

# Install ASP.NET Core
RUN aspnetcore_version=3.1.23 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='85390781e27d60bce8ebfe95468ddfa58b7538e42b5391f97a78547f34b792e1601d000a08a399f5599e55efdacc45ea6d7aa5d797e2063aa5063435615ddd61' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
