ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.2-alpine3.21-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.2 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='f96a66442f7db558e40491f1deba42a58b697286a8c592f20b9b17006040544c3cde3851dbe9d5dd0be4ff68666575a10a8e1225dc97069081a66726a9f0fc8b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.2-alpine3.21-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
