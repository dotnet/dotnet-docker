ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.7.24369.10 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='7f893320daced94a35c43668ecd31a83ff69cf34786a74d25e4c1e250247cbf0255205ea4dc8b157c1ab7f32847802a3cddb5a01ccee381b73dfd12b20335ac6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.7-noble-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.7.24369.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
