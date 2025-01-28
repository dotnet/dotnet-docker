ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-alpha.2.25073.4 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='ca6a318c736b8c7225c9d7cbfef32fb886da9a17ae64c6222c5247f17729b337899d42fb0ba9af34986059c693b7b6e0bb780d2997c0bc3d9300dae463c1e513' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-alpha.1-noble-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-alpha.2.25073.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
