ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/7.0.0-alpha.1.21551.1/dotnet-runtime-7.0.0-alpha.1.21551.1-linux-arm64.tar.gz \
    && dotnet_sha512='34d646134ae71513ac6f37775163ac2e855e6e6e9c91d9d9a9e9cfe5d2d24232a79318b1df4d439e027b38eed1fa8b099249447dc70fca1bfb425fb661ae65f6' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-alpha.1-bullseye-slim-arm64v8

ENV DOTNET_VERSION=7.0.0-alpha.1.21551.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
