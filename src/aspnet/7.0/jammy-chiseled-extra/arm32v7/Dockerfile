ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.20 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='e82bb0f1d52d91fea6bed419a18561282ed0e9de36e564e641e296c917f7f65fc7555ccc046fed675ad67b4e7c50a70f0f48e8559ff4f2a9bbad2bc62dcf6ab3' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.20-jammy-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.20

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
