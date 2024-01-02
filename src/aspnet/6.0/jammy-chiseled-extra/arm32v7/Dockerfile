ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.25 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm.tar.gz \
    && aspnetcore_sha512='917b3207a20382d863dd33ad19bc075df49000726dd55908cec506f96d6ba406d5b60cf15bebc6559f128bb83f9dbce208e7f44ec87a8d183704a9314e0dc4ec' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.25-jammy-chiseled-extra-arm32v7

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.25

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
