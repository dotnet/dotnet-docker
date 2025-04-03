ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM $REPO:8.0.15-alpine3.21-arm32v7 AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.15 \
    && wget -O aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-musl-arm.tar.gz \
    && aspnetcore_sha512='fd6890890dd0fccd13d817011e536214dded1b0d18eacd346954751a4642e0d6e5bcdcfaa485964085f5700f41ca009bd94f948ffce705a21ffbcc419ca2daa3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.15-alpine3.21-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
