ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve .NET Runtime
RUN dotnet_version=7.0.0-preview.5.22301.12 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-arm64.tar.gz \
    && dotnet_sha512='0751be17efa3191e6c9dd3bf0b7f1f8fd21028282b9eacf6786c6f61c7898199d15ad4719686e0437e4ecdd0c2d85830344732afb5e2f4579e89aa410f75ee4f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:7.0.0-preview.5-jammy-arm64v8

# .NET Runtime version
ENV DOTNET_VERSION=7.0.0-preview.5.22301.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
