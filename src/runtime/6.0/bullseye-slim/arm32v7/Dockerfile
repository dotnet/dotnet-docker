ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21419.17/dotnet-runtime-6.0.0-rc.1.21419.17-linux-arm.tar.gz \
    && dotnet_sha512='94c2f61a191a5bc2b51af355388120fa63ff5eeccc97f87d7cfb85bfb3f4271f9e0d5c689fda9620f0198c79fda2aeee43f30c90224cf944075fbb9e16e0f20f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-rc.1.21419.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
