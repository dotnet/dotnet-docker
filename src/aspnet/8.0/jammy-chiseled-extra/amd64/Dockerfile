ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.1 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='64eecc0fc50f8c68205123355c43eae5ee29b7f6061a260315818960153fdf25f2bb25a51dd3f051e2362e228c032f2d0b9e7b6b476ac52141c17cfd8de0bfd2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.1-jammy-chiseled-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
