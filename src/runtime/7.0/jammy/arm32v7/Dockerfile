ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=7.0.0-rc.1.22426.10 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm.tar.gz \
    && dotnet_sha512='dabff370d55a2cdf3623654f50eea950d85e7e986dda95e1beb7995dc3959ced1a1232a7dc622d3fe867e6fd12dfb074405f6761bb5ff4a83aa70a64b66de48f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-rc.1-jammy-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-rc.1.22426.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
