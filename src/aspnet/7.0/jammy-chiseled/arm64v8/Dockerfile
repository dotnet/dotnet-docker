ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.0-rc.2.22423.1 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='91fa2c918942dd58545d25093045ae925941dc7eedf0b36bf87d642915e3ca61d119e456c751e0bcdc3d9a0e1490429a359eaa504e1e2f97b73de66a0d4c908d' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.0-rc.1-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.0-rc.2.22423.1

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
