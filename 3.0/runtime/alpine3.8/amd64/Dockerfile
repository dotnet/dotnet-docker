ARG REPO=microsoft/dotnet
FROM $REPO:3.0-runtime-deps-alpine3.8

# Install .NET Core
ENV DOTNET_VERSION 3.0.0-preview-27122-01

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='ee1099bac0c08e17d5313303fd471ff525437ea73f830893ff9c5cf1547d58c1d0a981b9fd6dfdd0e2988e44998df77018864e24140fc8444a83a3b541848f8b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
