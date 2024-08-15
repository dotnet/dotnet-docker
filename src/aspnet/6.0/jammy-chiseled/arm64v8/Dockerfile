ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.33 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='7a60a77a306070a3b94db1acfa73938b6880cd079bdac3e5cab174a47af467b9208e9f41d8e12e080831d528151cdaa5b660bea5aa6fe537ec144543c0fffd95' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.33-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.33

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
