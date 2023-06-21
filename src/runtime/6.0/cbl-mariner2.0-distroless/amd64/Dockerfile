ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve .NET Runtime
RUN dotnet_version=6.0.19 \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-x64.tar.gz \
    && dotnet_sha512='6e8e1db8b247c92c8de4b476e06ad464b0bd664919394afd5fb3962db477490e54865abd2510c29457efdef3be23f0ea4683d6caabcc74a6d7abddce4b4a154d' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# .NET runtime image
FROM $REPO:6.0.19-cbl-mariner2.0-distroless-amd64

# .NET Runtime version
ENV DOTNET_VERSION=6.0.19

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]
