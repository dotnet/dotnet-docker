ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=6.0.2 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='fa42a686122655a2a7dedc2415bdd618ff06b0a57132d7d98ad79f25c40f3f9edb953ef1ac93fa1a1ff64cffb4c5276dd7586ba5d16c5a7960913e1c5dd646a4' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.2-bullseye-slim-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
