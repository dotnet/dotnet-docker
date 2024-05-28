ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.6 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='16cd54c431d80710a06037f8ea593e04764a80cbaad75e1db4225fbe3e7fce4c4d279f40757b9811e1c092436d2a1ca3be64c74cb190ebf78418a9865992ad12' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.6-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
