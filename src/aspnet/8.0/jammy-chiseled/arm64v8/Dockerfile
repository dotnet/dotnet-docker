ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.11 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='75b5888b7d65cf9e971925e48962c0822f630390a3f0f04ce1d84546990fed312e8ae8513c82caeada145c2ac8de2b229fd1dad2d2df36c8e9db0df9f65595ac' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.11-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
