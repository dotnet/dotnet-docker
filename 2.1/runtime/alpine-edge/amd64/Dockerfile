FROM microsoft/dotnet-nightly:2.1-runtime-deps-alpine-edge

# Install .NET Core
ENV DOTNET_VERSION 2.1.2

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='092c966af4e3b697aaff06315c3eb3e342651643d3a1929bd33bb5f638e73989944ecdfb02ac18b251b797ff4cadcb312f4be9fe8627b4dd4b8dd8b51ea59ba1' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
