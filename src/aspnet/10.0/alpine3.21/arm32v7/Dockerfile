ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.4-alpine3.21-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.4.25258.110 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='d6b6e4f2f548b0de2ba48c85a55de60675243d91b9e433aa2c4c036679aeb814236f9cf4f38509da5d471352a3845a830e1e02e5bc5e971acdb841567285626d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.4-alpine3.21-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.4.25258.110

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
