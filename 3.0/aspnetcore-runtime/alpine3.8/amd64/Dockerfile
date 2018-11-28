FROM microsoft/dotnet-nightly:3.0-runtime-deps-alpine3.8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 3.0.0-preview-18577-0031

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='0c5e36534a17f9d5c199f33ffca13a0b40bca08c2043b07591eb2950e9f37c8a55740f36da77ce3fee8c201a6930e3a2a42156594498fe7ad62101a4e30dde9d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
