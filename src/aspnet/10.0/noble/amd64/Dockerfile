ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.1.25107.4 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='77d19025e07f425adb82e6c75d77a690bcebb26c35cc126ea9bbe2c22c238ee8ba2b33908698f5257e2548f481de0b95d0b2419ac8073bd3333d5240e9e86ed0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.1-noble-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25107.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
