ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21425.15/dotnet-runtime-6.0.0-rc.1.21425.15-linux-arm64.tar.gz \
    && dotnet_sha512='cb80e5f8f8628ff65ce0a81a7af44e47fe0a7249769fdcb3ecbfda4e46955dfdec30c68455e5495461ca7b1bf81ef51a7335607cb17ac1a2682d8783f3d4763a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.1.21425.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
