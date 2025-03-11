ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.14-alpine3.21-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.14 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='6e1be0e3106914fe86ddc7eb7c7531bf79435edb44c293b5b2175089c1659dc2f9d313ce203e04f04b7849feb544d43568c74eae1f7e801dac9d742d93b7c6df' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.14-alpine3.21-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.14

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
