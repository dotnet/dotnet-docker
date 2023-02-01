ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.1.23080.16 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='a168ea918251d9ce64941256abfb968ff32ac7814df6bd6ac3bcb9d59550e4993fddb78f7986fe92974602b628508c747ddd16fbe8ec4b3bbf2a70ee92bf18c1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-alpha.1-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.1.23080.16

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
