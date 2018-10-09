FROM microsoft/dotnet-nightly:2.2-runtime-deps-alpine3.8

# Install .NET Core
ENV DOTNET_VERSION 2.2.0-preview3-27008-03

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='858267ef9c69b7f86d41113057b708ac4fba44a51033e0eccc25e3f12125c0bcd693ba406723977b0b73128c8e1318c342f8a93205455f2ce7fea5beec8cc226' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
