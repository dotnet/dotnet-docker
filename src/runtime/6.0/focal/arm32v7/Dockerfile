ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.2.21480.5/dotnet-runtime-6.0.0-rc.2.21480.5-linux-arm.tar.gz \
    && dotnet_sha512='50447f1a5d121c7b5cc34678fade10933711f2a14f9f8be6930bfaefadcd31d7d20518ba48b4335ac69eeec24586eb00c239157eb8df139cfdde23d8820b1e76' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.2-focal-arm32v7

ENV DOTNET_VERSION=6.0.0-rc.2.21480.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
