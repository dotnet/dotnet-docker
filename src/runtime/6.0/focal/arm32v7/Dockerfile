ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21424.15/dotnet-runtime-6.0.0-rc.1.21424.15-linux-arm.tar.gz \
    && dotnet_sha512='f668859bee6936bd3c5e93ee3026a5eeba6347cd3914da7e0cd8def233cf7074408d984b4dd3aefa98e333e7536d2bcb8b1f857c3377a7fe8bcf7e3585fe6b17' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-focal-arm32v7

ENV DOTNET_VERSION=6.0.0-rc.1.21424.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
