ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.11/dotnet-runtime-5.0.11-linux-arm.tar.gz \
    && dotnet_sha512='2b1aa807657fb195e15af18b96ccd14789cabe7375782fb1d5b4834d64057a64474c735743b8a9b69c70d55882e66be9c17e858f3dfef2d4567e7287fcf47943' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.11-bullseye-slim-arm32v7

ENV DOTNET_VERSION=5.0.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
