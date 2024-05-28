ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.20 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='62ed9743972043a72e48d5aa2f7fdf3483cf684a32b051315004d1c778e9712bf66e5e7a97a5a53993fa8e92daf5bacaf2cdb3eae44bb9a9e25532b9a80f4f70' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.20-jammy-chiseled-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.20

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
