ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.3.25172.1 \
    && wget -O aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='f1b06e651590ba866dc312da08a85b4b3e4c8d71a1dce9d5719e1e86f5d7ad646913dead9552d74eeedd7cb70a349d10fd54c77dbc9ad09b34327f739fc45092' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.3.25172.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
