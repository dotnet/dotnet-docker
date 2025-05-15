ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.4.25258.107 \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='df67703c695d43de48c18c0385a5c6aafad3705c723809336ed5100bd5b79593804de8125cc7623ad74a43c2b9e1734999a4ffad91689f83e9bcc5fd352235d2' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.4-noble-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.4.25258.107

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
