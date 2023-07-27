ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=8.0.0-preview.7.23375.9  \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz \
    && dotnet_sha512='a6831c7d5e0b2516290224c57c8a410ad6943100cc3cf45be25cbf3a7b45e3ec933dc5f5d291c6d421ea9d2bf3c19f1102a9f5ad5d8a2ae4ba72ab48d644d325' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:8.0.0-preview.7-cbl-mariner2.0-distroless-arm64v8

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.0-preview.7.23375.6 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.0-preview.7.23375.9

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
