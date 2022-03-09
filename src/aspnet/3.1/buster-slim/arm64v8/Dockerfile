ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.23-buster-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.23 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='2fd873ba9362d4c63d3583444e80910115c06a9378c528b06d40b0a49b6626849a9001bf35db32f8959084a33dd4da6b16116e7d240c79920bfd8af0c84b1f0d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
