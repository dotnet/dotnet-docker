ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-focal

# Install ASP.NET Core
RUN aspnetcore_version=3.1.4 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='87eac324d412f6e72a60fe454c099acfeec1706e2f741bdbba6d972bca7a6de1c0690f8594cefeecb6c47b331ed4ecac2c2eb3a4e18b4262a999cf6cca6f1320' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
