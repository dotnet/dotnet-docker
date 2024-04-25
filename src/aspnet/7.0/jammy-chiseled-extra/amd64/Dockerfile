ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.18 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='d2c3af93b9b8280c4e519f052f443e0504ccf7a04b3eef60500b2e3a8874a60a3a545ca936f8433887bfa6388106c19283c5a6a2c78ffdc19889bde3edbefbda' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.18-jammy-chiseled-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.18

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
