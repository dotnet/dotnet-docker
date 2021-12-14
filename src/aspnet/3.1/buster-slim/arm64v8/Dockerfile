ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:3.1.22-buster-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.22 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='b60afc6277fbf256c6c8ff17cb644d0e893054d0f017d7fe68ed3acc5bc1276636cdf11e89148bd2bea8cee35acee688224d64cc3a468152ee8c84a9a33b4a2e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
