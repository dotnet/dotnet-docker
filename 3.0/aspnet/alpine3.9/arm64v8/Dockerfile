ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.0-alpine3.9-arm64v8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview6.19305.13

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='dc5d6f93060ba7d733b0ca48a1577f53aa28bf66711f404ab5ccd10a7258c1d6acdda3c4536838b485fffd250e5cdb1a95b1ffc55d075da7dfeb2fe5e49d0910' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
