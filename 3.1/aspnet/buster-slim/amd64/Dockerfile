ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-buster-slim

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview3.19530.9

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='38ebb5fa8c00da0fe241ccd408dd490c2803b04aeacff25875c3492eb1a4f4022279e12de920a3675cedab34fe894c4c5c67cad4e78029b64f03ac8634f00b83' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
