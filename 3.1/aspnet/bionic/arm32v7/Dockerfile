ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic-arm32v7

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19503.31

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-arm.tar.gz \
    && aspnetcore_sha512='62dad90e367ab04e3a2d047b930fadafad04a9cf4a74c29a2466888e7d68e51ca7d93d24d588c2f5ff1981caca8b06933cabfd1b491f9e4e3b875b343cbcb663' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
