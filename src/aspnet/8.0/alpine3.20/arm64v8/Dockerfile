ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.13-alpine3.20-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.13 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='cec1eefebff9c3b8582417fc883cd19a5cf1741f00ed5df5e42a239398c3286f9f8e1fe6c9a65ffbd76fa95318cf23f7941f47f6132f03afbc1fafa3a48601c7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /usr/share/dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.13-alpine3.20-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.13

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
