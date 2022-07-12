ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.27-bionic-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.27 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='3f2aec61ea701dd4aa467863eb841d55f059e4dbf5dc54101072b039106699e4d7ecf62094906cc25473b4bdc8c6682522cd020f9af00a5a67027b2c523cdbc7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
