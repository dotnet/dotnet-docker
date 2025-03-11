ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.3 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='2a7508de9795d8238a1d4ff7e74df819538ca47d6ff8663ddb27b5f7514dcf487e7eed22d8c84258cad3fb30066621a6c79e08f3b060f15d64e85145b376aef0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.3-noble-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
