ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.6.25274.104 \
    && curl --fail --show-error --location --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='f3f5c03baa6b2d4f5497d555cb190aaa023aedc6f1732fb5eb8a956529fb80ce14d3907974e5b64054696d4ee0e8006abd0c2bc596e50ecbc67fce4e838c3a3f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum --check - \
    && mkdir -p /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.6-noble-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.6.25274.104

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
