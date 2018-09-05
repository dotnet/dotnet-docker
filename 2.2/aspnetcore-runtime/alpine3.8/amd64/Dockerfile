FROM microsoft/dotnet:2.2-runtime-deps-alpine3.8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.2.0-preview2-35157

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='bff1d30755a4918d42107248b8fd94d4a0b782d8715ff2c2b5a0a547e5bf88ceace574c354a2ba314390909bc2d915dbb048d3304492685b204a14b1ca2ab6b0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
