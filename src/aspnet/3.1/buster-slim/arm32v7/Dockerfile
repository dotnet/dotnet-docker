ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.30-buster-slim-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.30 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='33e3a6b2e5cffc019a25c4d580047bbf6e927e71da62e043984e76dc4d17d76dfbf8a1576d741038c3bd16ecd6f09c395c08128d85a69adf4cc46a5f803d2853' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
