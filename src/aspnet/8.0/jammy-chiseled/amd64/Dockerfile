ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='c0aa3a926d6c2bc0d4cc14f5d7677a4592111bf3ebefa65c5273c4b979a6e2b5d58305a5aaf4ac78f593b46605ec02f40b610dcbff070b1d8cf8ddc656cac7dc' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
