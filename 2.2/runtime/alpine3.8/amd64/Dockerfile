FROM microsoft/dotnet-nightly:2.2-runtime-deps-alpine3.8

# Install .NET Core
ENV DOTNET_VERSION 2.2.0-preview3-27002-02

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='6df72a82eaac37ddabb377e97a43640919c3e36e7bd8cd6de72875b2d7dd2546c0fccccdc6added437df9663236b7f03358dce05e7fade41d4b590bb30cd2730' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
