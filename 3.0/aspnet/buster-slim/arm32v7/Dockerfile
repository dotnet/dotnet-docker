ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-buster-slim-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview7.19319.1

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='dc109b745087a1b84b4e0096ba9725ab90c1f2e2c7f26a4207cc8c1c9595f6aa6bad8292e76528bbd0817c0d0558c87a534c591f2d0f5c60a4a74b0f3ad798a9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
