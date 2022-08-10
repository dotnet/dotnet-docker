ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.8 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='07babe85c8872ca303a17268b0d23c382a9ac49f8b923c45c496db039f6c01094303cd18cd31f964ba7369bb993c896eeadbb7e458a77d5b86992222b91db52c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.8-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
