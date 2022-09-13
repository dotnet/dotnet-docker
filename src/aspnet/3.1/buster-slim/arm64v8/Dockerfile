ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.29-buster-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.29 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='7cf6bccb85b39990d19ed5f42c8907e9fc615358330a060e9f93455c277143ec261f5255b6b05b081ade155f7965db5b092c956b0c77b2ebc9e2dae065f8e977' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
