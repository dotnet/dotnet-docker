ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-preview.2.24120.6 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='e02c20f3f6cdf7631e01a93659940336701fd262202b8cecfb7b8fe8cf970fb3ee947c6551d2efe494e8b0d5bd78122cbe804e4b1fe524ff9fdffd14a5775a7f' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-preview.2-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-preview.2.24120.6

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
