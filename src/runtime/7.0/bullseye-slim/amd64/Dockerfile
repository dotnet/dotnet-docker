ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl AS installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22062.8/dotnet-runtime-7.0.0-alpha.1.22062.8-linux-x64.tar.gz \
    && dotnet_sha512='1963c41cd873609f0c9718e8dd03a7c28c26fa7f27dbb772f60f9863c9ee8dfe22c5d88604e9b9021b45ce9cce39dc7a675180d700014faf43f047a23d4838cd' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.22062.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
