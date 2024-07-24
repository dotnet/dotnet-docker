ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.7.24372.2 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='ec60882cb9582297634c3ffcb7a718bfbe2c22464f8b80318ceaf9adef0bcdd73f586a31998656986a463e04ed500750db04c29fa521f25035a927a4a66b07c1' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.7-noble-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.7.24372.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
