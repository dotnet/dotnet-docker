ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.4.23251.12 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='eea2736eea5a6e75acc94d8d670b428f1eecd23cc2855f19b708206b5b7e608c4128c01d23a70caec33966e0173ef16960ef329bbb41baecea5b684627f95917' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.4-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.4.23251.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
