ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.10-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview9.19415.11

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='a77df473be8bab82809d20bce31fa6fc38748c60170c0404cf5a71b65fb50b6888797dff2fc8668445be90e7b9749f49d45f975f59806c733971b64932064462' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
