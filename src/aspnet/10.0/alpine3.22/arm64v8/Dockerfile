ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.6-alpine3.22-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.6.25274.104 \
    && wget --output-document aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='110c72b667853d463e8247478087d88dceff244f9cfb86cc3bebaba27ac6f8f3d748d2d3a6af05305b7d840e7c0579c50b6b953ccd95582b8ad85f71de26921d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum --check - \
    && mkdir -p /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.6-alpine3.22-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.6.25274.104

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
