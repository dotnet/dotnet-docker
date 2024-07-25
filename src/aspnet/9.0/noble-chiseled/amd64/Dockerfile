ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rc.1.24374.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='58b8723b879ea0a5ea7ab3a5f66c7e01cd12308a48ff9ee0db61d0c26b1ad3712b64acac12a0969107b233779eb4c76e7ee238fba9479a5ca62a37fd5501c08b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.7-noble-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rc.1.24374.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
