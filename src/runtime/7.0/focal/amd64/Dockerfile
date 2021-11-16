ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/7.0.0-alpha.1.21551.1/dotnet-runtime-7.0.0-alpha.1.21551.1-linux-x64.tar.gz \
    && dotnet_sha512='b67273e0e362ff9cda5752107cd79e0911c8420b9b6e9f5919426a48e8782e2ebe4bad9230c56d5b78fc95269d07d60e5af0ddfabf5a00ca505b84365edf481b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-focal-amd64

ENV DOTNET_VERSION=7.0.0-alpha.1.21551.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
