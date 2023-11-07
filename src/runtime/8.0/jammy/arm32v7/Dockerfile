ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.0-rtm.23523.12 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-8.0.0-linux-arm.tar.gz \
    && dotnet_sha512='358c4725bcba65db7eecf277235f65c5b78565ea012435e82603a0e9e0ae21d751e20c2b8d57aa61e725a2377f20d4348c711667cf42a9e0c40cacf0239d7593' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.0-jammy-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=8.0.0-rtm.23523.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
