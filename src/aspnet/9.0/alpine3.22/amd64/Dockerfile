ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.5-alpine3.22-amd64 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.5 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-x64.tar.gz \
    && aspnetcore_sha512='880d6c516af46dda0686c0e56a55cad5d8c34bfe3cacdbee68a9d21a5ff9e9ee93a6c427ef0aabd4c45ab95aa772edc9b89927f38d07619db6219c93e6f6e43b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.5-alpine3.22-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
