ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.2.24072.6 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='9595da8b76eb69d549d4ee0f37afe5f5550bf5b737dbc0816d45fcb976c1e6b9acee7bce5038a32d01549e1167de5b44f0efad2a63b8c321c1e03fbbdb2d6f50' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.1-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.2.24072.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
