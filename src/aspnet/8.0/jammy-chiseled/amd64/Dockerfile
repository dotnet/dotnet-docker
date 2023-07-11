ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.6.23329.11 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='767ee4f67777d53edd322c771baa70d592e9fec99dce66e66a67fdd3c377b3f791f0f89b7dd5bbce932f499266f52f21449728849def706d976b55dafe2e6551' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.6-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.6.23329.11

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
