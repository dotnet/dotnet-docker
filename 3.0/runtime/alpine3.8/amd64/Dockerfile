FROM microsoft/dotnet-nightly:3.0-runtime-deps-alpine3.8

# Install .NET Core
ENV DOTNET_VERSION 3.0.0-preview1-26829-01

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='9e8cbef6bceaa8bd844a4a55419827a2786b7edde92ed30ca406729bb513f347236138cb351edb1a052120b63bbfb24ad9fee8d152e90e09767cd720e0d620eb' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
