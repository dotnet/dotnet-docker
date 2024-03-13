ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.3.24161.11 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='f82f3609fd00d0169a3b489a9eae18397a210553a6e76cfa9df57163360a4c18c8421810079ccc41e4e190d189bfde343eca98b2e74c0eb09c799f02a692dbf1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.3-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.3.24161.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
