ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=9.0.0-rc.2.24474.3  \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz \
    && dotnet_sha512='0b215df947c2bef3cdd9bd298cea18f6c3f5e21fa46d2ab9f6faec3b9f7f062fb35819432e7e1015f27f793b4850f2cfa40040e60f28fbbfec258acd43094695' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:9.0.0-rc.2-azurelinux3.0-distroless-extra-amd64

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-rc.2.24473.5 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-rc.2.24474.3

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
