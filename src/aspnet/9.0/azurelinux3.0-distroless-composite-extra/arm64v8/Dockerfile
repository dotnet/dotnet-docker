ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM azurelinuxpreview.azurecr.io/public/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=9.0.0-preview.7.24359.10  \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz \
    && dotnet_sha512='1b182b4595ad80301dde528987b6c9d21773c1e1c984d8125a987c5a632a54bf225e61bd3fb81b913a1dedfa8dcd943d553cc9f4e42bba8168baac9bbf6677e7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:9.0.0-preview.7-azurelinux3.0-distroless-arm64v8

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-preview.7.24357.2 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-preview.7.24359.10

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
