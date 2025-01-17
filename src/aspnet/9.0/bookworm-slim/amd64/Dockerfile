ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:bookworm-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.1 \
    && curl -fSL --output aspnetcore.tar.gz https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='e5fc3093aed5756deae3e61f98b9f4bb0c847319db30cbd1668c2511e06529c2f6a5e1917ec776fe2b36a1f7bb7e009fc925fee57f87696a8d502a6c8f5dc613' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.1-bookworm-slim-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.1

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
