ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=11.0.0-alpha.1.25609.108 \
    && curl --fail --show-error --location \
        --remote-name https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz \
        --remote-name https://ci.dot.net/public-checksums/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz.sha512 \
    && echo "$(cat aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz.sha512)  aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz" | sha512sum -c - \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz --directory /usr/share/dotnet \
    && rm \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz.sha512

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:11.0.0-alpha.1-azurelinux3.0-distroless-arm64v8

ENV \
    # .NET Runtime version
    DOTNET_VERSION=11.0.0-alpha.1 \
    # ASP.NET Core version
    ASPNET_VERSION=11.0.0-alpha.1

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
