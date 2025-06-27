ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.6-alpine3.22-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.6.25326.107 \
    && wget --output-document dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='ddfe767c4067ed50f6706ceeda7c0792f73797d23455f920fa8e1f455ff8ed6213f641cb57fcf199e3ac3a77bf8d0e3905300a6fb00360a7192e012f161cd0ca' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.6-alpine3.22-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.6.25326.107

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
