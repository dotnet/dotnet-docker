ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-alpha.1.24068.16 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='8150a3c252368257eb8edd159bf973e667c09a3e875f37e898b2a1ce2fba924f6a92de469468be64338620e8ea94d58caf1b0e7c4514c9e7bff4059c1cb43e27' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-alpha.1-jammy-chiseled-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-alpha.1.24068.16

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
