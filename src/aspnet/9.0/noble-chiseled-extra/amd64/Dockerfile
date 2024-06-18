ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.6.24309.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='9d68004d403b5abc57a3e6c4e67aad76e61ebb3999aa277989603a76dad38ceafcac26ce2b5a95cffb3e86925a4af0494d1ffe0a7c9b0b2c5d5e742569b4b131' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.6-noble-chiseled-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.6.24309.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
