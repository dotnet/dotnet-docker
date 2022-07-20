ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:1.0 AS installer

RUN tdnf install -y \
        ca-certificates-microsoft \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=6.0.7-servicing.22322.2 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='996bdaf33be0a9c0f1e2d309b997e3a84a31e28d2424853d7fb1600212f4ce600ebe1b9615de5e46c17652f08ad0d7ecd4b3619217c9624b875a26a553f370d8' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:6.0.7-cbl-mariner1.0-distroless-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.7-servicing.22322.2

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]
