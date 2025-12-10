ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.1 \
    && curl --fail --show-error --location \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz.sha512 \
    && sha512sum -c aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz.sha512 \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz --directory /usr/share/dotnet \
    && rm \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz.sha512

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:10.0.1-azurelinux3.0-distroless-amd64

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.1 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.1

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
