ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.14-alpine3.21-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.14 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='dbc6dbd4bbae6137bb08e115dd2ad675c5373c3d573023cef0bbdc05000e4cadf2f31b8c4425ae086be1712a1cbb215bfb2ad19cd4e65c4b13e4ea08f0408d73' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.14-alpine3.21-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.14

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
