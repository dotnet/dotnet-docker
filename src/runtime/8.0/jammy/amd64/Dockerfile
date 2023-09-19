ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=8.0.0-rc.2.23466.4 \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='809b1313f52b92e00dd29f467f02354f861c6f6a3f44b6a2ce551dd8a23390df913f0e6f4dc8b785fc00ba08e243b770e2cccdb62d9e5123ebba1f527375deed' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:8.0.0-rc.2-jammy-amd64

# .NET Runtime version
ENV DOTNET_VERSION=8.0.0-rc.2.23466.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
