ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.7 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='5f1d31b0efc793655abf4289f8f1c7e8cd1ffabfd65b385b49e3f5232277c62ccfbbdad2a51731a8a88594a06c2c9774e38865cb3f7e19c9925a12b25b40b485' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.7-noble-chiseled-extra-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
