ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.1/dotnet-runtime-6.0.1-linux-x64.tar.gz \
    && dotnet_sha512='2a316e8cba20778b409b8f2a3810348e2805f35afad8aba77a67c4e6bb2c2091e60bc369df22554bb145a5fad0c50e20b39d350b98a85bd33566034a11230da7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.1-bullseye-slim-amd64

ENV DOTNET_VERSION=6.0.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
