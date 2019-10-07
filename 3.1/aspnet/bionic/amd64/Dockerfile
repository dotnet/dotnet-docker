ARG REPO=mcr.microsoft.com/dotnet/core/runtime
FROM $REPO:3.1-bionic

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.1.0-preview1.19506.2

RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-x64.tar.gz \
    && aspnetcore_sha512='8b0a386aadcc9f01acaeabec54fa808120a906f3bb3bd3468135403db9a0178c404554e9495551493d7b509aa46ffc7b1dd933c1e86cc8ebb41a64ebd3394dd3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz
