ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.17 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='0eda8538107e592abf47b391a37f13c58d6ceaf7f293ae594094aa954400fe7b6ab4eb6d723eb447f22e2b0f05d1c470907842fb2bbf6b63f66502b6b3f1cfa7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.17-jammy-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
