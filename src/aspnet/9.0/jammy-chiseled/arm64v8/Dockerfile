ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-alpha.1.24058.9 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='151881660850cb81b887f714659b7a8a344ebd320418bea361a662255a4a0443282d0e3aa28411493a50544630ae0a31217330dfb7b6871f04c3428fc9f80b4c' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-alpha.1-jammy-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-alpha.1.24058.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
