ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.17 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='a0cc7f76f24d123fbe787ff3b554736000c3f6b4f7b919819fb3039f6df4a15d28713a0a169c9493012e14afc3a0299f3d800d93d6749a70b567833ef3f3aeed' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.17-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.17

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
