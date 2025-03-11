ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=8.0.14  \
    && curl -fSL --output dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm64.tar.gz \
    && dotnet_sha512='fa99be0e1daa2d419f7c85d70a9648db798a51329743362db61eaa8fe5bbe4a355449b47ee2b7d52496abbee6f885dc154fa3be98dd4313d028382c132ffa61a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:8.0.14-azurelinux3.0-distroless-extra-arm64v8

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.14 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.14

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
