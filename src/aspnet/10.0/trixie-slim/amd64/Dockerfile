ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:trixie-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.6.25274.104 \
    && curl --fail --show-error --location --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='d77697b18f3d3f601be4a1cbd7c26100124766345124bd0aa9bcceb7ca2ba33d21491c9094079028dcebd94f00d385cdc32720f9c4ff47be4a5f39af63398b67' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.6-trixie-slim-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.6.25274.104

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
