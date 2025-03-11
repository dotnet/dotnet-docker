ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25160.5 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='b312e0101f931c9e79f9042f94ce83cd9eb6abeaee0819e88c8cd23032123b4992fb333a467f0213f0d7c9a96e6210ee585f74697d5edeeae29e149c8db1e5d8' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-noble-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25160.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
