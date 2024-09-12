ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rc.2.24461.26 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='4eea8f37ce962067baac747a23951fc130b385a7f5b485c7563e2a51f407f5d8667d93ba6fac59e951fd5fa1ee6d5ae52ed93c308b0b11cdb52fe9250c121cbd' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-rc.2-noble-chiseled-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rc.2.24461.26

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
