ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=8.0.0-preview.4.23220.8 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='0efb1ec6e3f64108e59f569cc028fb1409810fcde833b073e57578d8d6ba9cb860b6b7db13c13d29bc6d478dfa1f7e591147db10096f1018e28ecd1d6a2b335e' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:8.0.0-preview.4-jammy-chiseled-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=8.0.0-preview.4.23220.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
