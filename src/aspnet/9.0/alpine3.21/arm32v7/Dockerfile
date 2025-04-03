ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.4-alpine3.21-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.4 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='11cea0b91ae542bc4d2fc451247595071eeb01f801d3305f9c9ccda7c04b4b1c327ee75a21452ac1b584a50a639e6d622272fc7ab9cbb744a8bb691b45baf2c2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.4-alpine3.21-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
