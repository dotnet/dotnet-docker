ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1-focal

# Install ASP.NET Core
RUN aspnetcore_version=3.1.13 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='77c5475e0be0ca78a01c728f4dc5b58b57a721605ab4b96aaa9585d5e83a711488edc78a9fcd37bb68b499d711cfe18a9671b4397a591a0632d4f739f02b9f5c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
