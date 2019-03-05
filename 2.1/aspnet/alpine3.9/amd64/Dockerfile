ARG REPO=mcr.microsoft.com/dotnet/core/runtime-deps
FROM $REPO:2.1-alpine3.9

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.1.9

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='d195124709b99cad213d86595ee71126e5802006f053751a4d836d23e506d0fdb6a6dfcc90b28041c71f5623bf18978226bb2fe144b5436f93718daa13a44673' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
