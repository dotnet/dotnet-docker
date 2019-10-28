ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview2.19525.8

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm64.tar.gz \
    && aspnetcore_sha512='fd63b27b8b37c3dcf871ca5fae333320d6ee9e214dbfe56fa71124328e130ff3534d4aea6577aa1615d130cad882dab3fe0501b5155e2033bcf6bcbf1b5bef2e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
