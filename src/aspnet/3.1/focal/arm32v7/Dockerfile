ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.32-focal-arm32v7

# Install ASP.NET Core
RUN aspnetcore_version=3.1.32 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='f984a6bf0d0c917fc74bee1d03af79a7e6ca6ce3bd722b14c0cf82641fdadb2d48014ea30989239f5082e4320668172409ee8a99c4acf7d9db155405187d7578' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
