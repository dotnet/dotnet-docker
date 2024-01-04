ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.15 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='34ba8038fa1861d44711302225ec47fcf0c1bc1a3c75fc7ce3b9f054ca46ec627e1864e72f47bb79c03f49b349ca6821567b04f9f3aac80f49afdabf97105ddb' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.15-jammy-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
