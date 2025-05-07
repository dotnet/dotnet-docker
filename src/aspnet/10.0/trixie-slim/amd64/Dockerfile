ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:trixie-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.4.25255.103 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='d27b04f3b06bd733344f0fda574fe61874467d9597fdec16395cce630e19cd07630109fcb9e69dabeced4e3089c0f13bf400a7d8f09783de37c865bfc2e0df55' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.3-trixie-slim-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.4.25255.103

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
