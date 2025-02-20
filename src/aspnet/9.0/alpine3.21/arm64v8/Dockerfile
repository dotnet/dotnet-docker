ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:9.0.2-alpine3.21-arm64v8 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.2 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm64.tar.gz \
    && aspnetcore_sha512='5e74871d9133c52389559eb34ee82ed7b8ff2ee990857ef80ebf2b27c405e0dee1ae951c9a87355168901d98acfbe37bd6d516d6a0b4f5f495c58326243c0630' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.2-alpine3.21-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
