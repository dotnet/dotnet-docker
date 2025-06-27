ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.6-alpine3.22-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.6.25326.107 \
    && wget --output-document aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='3f315be02db29409f0e86da1a04458fc63a365b6a72bfea6afde58e790f862646a7df7fda7477b5a6a8e6bbb306f85b090460a98f00c8fc91d91a347518dc553' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore.tar.gz --directory /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.6-alpine3.22-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.6.25326.107

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
