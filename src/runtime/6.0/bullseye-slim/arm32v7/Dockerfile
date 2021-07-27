ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21376.17/dotnet-runtime-6.0.0-preview.7.21376.17-linux-arm.tar.gz \
    && dotnet_sha512='bf1eb1713a80eb8a37f65a0bdfb9b4e9e2c59bd791dc817748c28ec5c5eb2e47463a9e799144e0f3c8e04d209a3ea985fa58ab5fd630e70cf25e74cd2d7f1305' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-preview.7.21376.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
