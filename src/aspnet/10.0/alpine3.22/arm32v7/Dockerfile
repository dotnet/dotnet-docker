ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.6-alpine3.22-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.6.25326.107 \
    && wget --output-document aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='ddb02abe51485d080c565881f235a2287bc7352cbf19fe8373bbad0c6b86b751942e17a3b08a340483eb0d27cad62c5f09e1581bbf538cdc696e8e2aed77baae' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.6-alpine3.22-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.6.25326.107

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
