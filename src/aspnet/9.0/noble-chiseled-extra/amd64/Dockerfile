ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rc.2.24460.5 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='b156cf6e20db73523179379ffa1698f0dae1197fb1cb0a22b51e2d6baac066679719b39c39168811e457a836f3ffc53618c12c0832d3b6d95e96a970edebb1b2' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-rc.2-noble-chiseled-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rc.2.24460.5

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
