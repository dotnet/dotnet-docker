ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim-arm64v8

# Install ASP.NET Core
RUN aspnetcore_version=3.1.1 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='7f44e4c134fba9f4e41c4b0148ceaf1551cf06b534f27eaf3c4533b211b35faf793fc8cee8d39bee37c908cd0fa0f220aeca1c713d2472e95662048a9ee0bb57' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
