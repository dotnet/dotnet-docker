ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.7 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='3d0216b153a74689d9ea1986b33e3747e62bf76a938769b913a32a7f6b9d83a0b520b125e9f2496fbf9c60a1fdf87a132a0961317cbcf3d406906a111ce832f2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.7-jammy-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
