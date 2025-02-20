ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.1-alpine3.21-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.1.25107.4 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='eb981f101e3c2e33cdcb62a65939493d767f471af64c36dfaf6a08e6e0f9cdca6c890e6962ae0662d3f67470724615966dabc7e5a94d8bf8e96f405c30cd6224' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.1-alpine3.21-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.1.25107.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
