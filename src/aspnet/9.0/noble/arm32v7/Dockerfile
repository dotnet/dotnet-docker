ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.5 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='7885f86dc8cfd555701739f47e1c4740accfbb2aaeeef629c29fd75c211ca556edf8737c942718cf8b7cfa888600dd7ba8193a2be1bbbf0754ced952d76217c8' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.5-noble-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
