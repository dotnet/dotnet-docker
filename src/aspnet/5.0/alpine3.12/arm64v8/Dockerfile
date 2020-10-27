ARG REPO=mcr.microsoft.com/dotnet/runtime
FROM $REPO:5.0-alpine3.12-arm64v8

# Install ASP.NET Core
ENV ASPNET_VERSION 5.0.0-rtm.20526.5

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-5.0.0-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='fb0703d66b223fed4d9d7d18c943e3a56fe7048b747a98edfce5abbebf37bcbf9e76bba50822704d028ecd004a48d72a1cab9a1ef4f2a99acb3f7fa6480349d9' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
