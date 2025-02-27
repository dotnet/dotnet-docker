ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.2.25126.5 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='a170fda71d1770060c5cb6a500ea1fe93550ea49954ff03c9f5f34d70db5a7202282bf222595211bca258abb46e671e6c5bccff949cfa2dbe38a73cc98c5c6b5' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.2-noble-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.2.25126.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
