ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21430.11/dotnet-runtime-6.0.0-rc.1.21430.11-linux-arm64.tar.gz \
    && dotnet_sha512='81f13bac6b7ff5c00f2cad82bd0ae70ebfe112f090718f7fbbc65c66df976dc7eee38715f32b0776df20cacbfae811ace040639b91315b4ead2ae2ae07528a9f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.1.21430.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
