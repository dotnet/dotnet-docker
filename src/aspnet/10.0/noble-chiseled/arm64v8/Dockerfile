ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=10.0.0-preview.5.25269.109 \
    && curl -fSL --output aspnetcore.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='cc28ef5a62d12096d0072bfd4a54675818b30e8f617c57f60ed0bf386a76f7eb4dae56127663a6b488e875bdecc4bdb29ef7dfff4a8c464972097eaeb8c5ae62' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:10.0.0-preview.5-noble-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=10.0.0-preview.5.25269.109

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
