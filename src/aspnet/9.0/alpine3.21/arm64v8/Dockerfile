ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.5-alpine3.21-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.5 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='12e0502c11879a93e5a568d19b4a3b50a096425b924720cfdc4d2ce532345118542ea2ddc9cc409f9d3b706f6cac2952affa3d25317cbf240bd69d43159e1ceb' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.5-alpine3.21-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
