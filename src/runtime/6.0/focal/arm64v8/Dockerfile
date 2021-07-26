ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21373.17/dotnet-runtime-6.0.0-preview.7.21373.17-linux-arm64.tar.gz \
    && dotnet_sha512='ae4922bc791da712e7057fc3a649e2bfcf6d173f35cd765cefb8a73f268a586f4be070f77ecc3c99e49e6491be4fc68be06269e4705010baf491a8d83f9abb6b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-preview.7.21373.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
