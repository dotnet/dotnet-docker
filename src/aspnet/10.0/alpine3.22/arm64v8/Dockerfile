ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.5-alpine3.22-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.5.25280.105 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='5c70af629528c7de8c95761c1954049b8dafdf2cbf25103e36738e08c668bb223999b2f1e19f40a3a7ad35fc0c869afc440ae5768a257a7f9684fce657f38970' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.5-alpine3.22-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.5.25280.105

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
