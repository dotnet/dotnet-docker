ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG VARIANT

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='5fd4f650217c8531bf5fba9b851462e52f3a04a71c4c1d7b48be6e22970cb06981d1b4fbbfdf2c8aa3662c9921a3c3d91d61ab19debcb5df046e87df1849abd6' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-jammy-chiseled${VARIANT}-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
