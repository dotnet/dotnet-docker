ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-rtm.23518.3 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='a849db931157b6984e3f66132f551be71d8ade5394a83b8c4c3a2464c5f8e56fb6730305d8f345f8a0b4e4c3cbd2def6f916fefaa4556492eadb52a5aa5ec1fd' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-rtm.23518.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
