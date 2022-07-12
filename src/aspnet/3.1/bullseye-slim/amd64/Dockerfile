ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.27-bullseye-slim

# Install ASP.NET Core
RUN aspnetcore_version=3.1.27 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='80f300c9081d66ecf911707f39772c437e5e6a4d6603fef48300b7e94c29d5520de08ab2b6b687786e22460381477ed30a2a69245364d1b4cc0753b124caaceb' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
