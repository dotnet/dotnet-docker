ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.13 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='de5a3dcfabaa7f01aaffb78b2156d9d79d614f0e52ebe619657072568ab0fc47f4d53e7b577a7622392502cd3a739400af6c26999247cc50c29813b6a94cfcd0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.13-jammy-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
