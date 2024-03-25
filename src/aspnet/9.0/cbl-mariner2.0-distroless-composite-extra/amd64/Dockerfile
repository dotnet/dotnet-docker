ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM mcr.microsoft.com/cbl-mariner/base/core:2.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=9.0.0-preview.4.24173.3  \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz \
    && dotnet_sha512='971827c9cf7bad7aa05d5faac1caa23574df7724f97f0d374ffb3aa82fef70a9c35320a59b6d46b788c76903395c9f591b395e65d58f3b50c53d95d9f5889e52' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:9.0.0-preview.4-cbl-mariner2.0-distroless-extra-amd64

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-preview.4.24173.6 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-preview.4.24173.3

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
