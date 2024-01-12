ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.15 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='8aae979c0e9c90e781b8747aba5d7e09c9a81845b936c9185dc16d519db3a4ad9e219da4bffe13476baa81c7ff3e1637e8ef031be1f9f305f7d1681568ae3aed' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.15-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
