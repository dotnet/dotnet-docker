ARG REPO=mcr.microsoft.com/dotnet/runtime-deps
FROM $REPO:2.1-alpine3.12

# Install ASP.NET Core
ENV ASPNETCORE_VERSION=2.1.24

RUN wget -O aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='ac5ec463d4ec4f588bbd1ae4eeb43bc1d55b609cec1c5113f7ac414c1cc206d0343768c54ef8793b5423e27023b6419053d706a2aaeb9d4bfd0ed7f77e2249d4' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
