ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.18-buster-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.18 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='82f5513866138626bf12dc1f5e48684ada96870ab93acef0bf8a9ef0f05988bbea6bdbad02bd52b2c394ca9fdcdc8b924b5bba7994bfe1c034913a0d77ffd00a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
