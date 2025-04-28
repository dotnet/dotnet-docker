ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.4.25227.102 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='e069d77eb8e2297648229a575c2c713a6b128ae7697c251e8fc5131822af4fb79c3d6db9d99e9aeff221b9a2f29ac0d44b8f27300e060b8caeaed824c83eaabb' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.3-alpine3.21-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.4.25227.102

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
