FROM microsoft/dotnet-nightly:2.2-runtime-deps-alpine3.8

# Install ASP.NET Core
ENV ASPNETCORE_VERSION 2.2.0-preview3-35246

RUN wget -O aspnetcore.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/aspnetcore/Runtime/$ASPNETCORE_VERSION/aspnetcore-runtime-$ASPNETCORE_VERSION-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='522708d9ecc33f699a55b7f94d312506f5b81aa8f67033a9a66d7d097584d6277f24362548228bdf045e6d3a721c663d93739b0082261be7640f87e8aed60b62' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -zxf aspnetcore.tar.gz -C /usr/share/dotnet \
    && rm aspnetcore.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
