ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM mcr.microsoft.com/azurelinux/base/core:3.0 AS installer

RUN tdnf install -y \
        ca-certificates \
        gzip \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.3.25169.11 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='76568ef77cc05914880e1ebd9bce3e56501dbf4458464f46f7e5afb5a04c9981da551ff2784f30ef89d73f291b39754a17b144871def2a93d1d2410506bd26c0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.3-azurelinux3.0-distroless-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.3.25169.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
