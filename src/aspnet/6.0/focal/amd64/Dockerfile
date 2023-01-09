ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:focal-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.13 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='96239951972a04021b1953581c88a3e1e64553d05278fb92729c62827f38eeecd533845d6b0d3e9ba97e9d19701b9ee8e5c02d1ee54b8c14efe54c08c45c15a0' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.13-focal-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.13

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
