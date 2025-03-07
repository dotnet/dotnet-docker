ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.2-azurelinux3.0-amd64 AS installer

RUN tdnf install -y \
        tar \
    && tdnf clean all

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25156.10 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='c688aa2c7c5d2478f8bbac69f7b44ccefa5f4aab97f00fc24546ad4c4acba4350aa4d0a666d1df77c9274073d4e72895da5f565ca6307fb8820548b9c809b5a5' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-azurelinux3.0-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25156.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
