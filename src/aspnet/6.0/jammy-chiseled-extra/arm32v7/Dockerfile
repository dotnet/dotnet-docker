ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.26 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='1e994b745afb5a9cfaaf8c4a66154e9ef81d8c25d6bc9703134a62400242660dd55da0ebed96dfb81282d85161178425b04bebe2b1ec1f7e38ed776b1c001b7b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.26-jammy-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.26

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
