ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET Runtime
RUN curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/7.0.0-alpha.1.22060.15/dotnet-runtime-7.0.0-alpha.1.22060.15-linux-arm64.tar.gz \
    && dotnet_sha512='14b84fd21d6293370ac3b489584515fc9f5b26a26cc6ea00b213a5cb09f4f69bf8d6b4d6ee35514ff321d9ff489c62c62ccdc534707e39decd932d0aeb2a32cd' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm64v8

ENV DOTNET_VERSION=7.0.0-alpha.1.22060.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
