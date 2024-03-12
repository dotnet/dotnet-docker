ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.28 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='52675b81e026b4b673aedb2d9ee99a79ccb47eab090a059ef9b95615befc034ef7fbe674b01ae813870f73dcdbcfa32906969860a464aa5d356c004b6bfb201b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.28-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.28

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
