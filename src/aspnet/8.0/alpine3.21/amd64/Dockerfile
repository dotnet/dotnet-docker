ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.15-alpine3.21-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.15 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='af6f6d7f84b404f44f2f3322407b68f3962ddb06a0ea57db7713cef21980ad379919a2035c60aa7b028d0720381f5965474b64446c9b25a9e58289631e04c15a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.15-alpine3.21-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
