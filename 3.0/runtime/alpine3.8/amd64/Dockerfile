ARG REPO=microsoft/dotnet
FROM $REPO:3.0-runtime-deps-alpine3.8

# Install .NET Core
ENV DOTNET_VERSION 3.0.0-preview-27316-4

RUN wget -O dotnet.tar.gz https://dotnetcli.blob.core.windows.net/dotnet/Runtime/$DOTNET_VERSION/dotnet-runtime-$DOTNET_VERSION-linux-musl-x64.tar.gz \
    && dotnet_sha512='3cad11a3431ca73abf98d72b1448f3dba2837731df29653ead549d3ce10c329e7b3d0323ac03bf5a94f6ba78ae684328acde9335bbc4c67cff290cae0b3dcf96' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -C /usr/share/dotnet -xzf dotnet.tar.gz \
    && ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet \
    && rm dotnet.tar.gz
