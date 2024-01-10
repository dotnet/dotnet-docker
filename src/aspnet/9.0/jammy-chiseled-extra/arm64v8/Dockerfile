ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-alpha.1.24059.17 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='831a2924cfed5b73687c98a81d234c5bff9c09317c1cc69168bfcc3fab53c85b0448e2f54e8c0ae7c7640d7a7376f6b87ca71a80bd8f6e7c1da0e1639ccaa484' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-alpha.1-jammy-chiseled-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-alpha.1.24059.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
