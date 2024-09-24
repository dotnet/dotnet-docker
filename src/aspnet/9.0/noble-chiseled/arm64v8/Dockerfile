ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=9.0.0-rtm.24473.16 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='c68dcee288314581655615e9a50c61bd4628fc9f19554bc9bfaa881242fbe03944ccef8b77dec6f00aa5261db16e4e5be724e333e5852ec1e3f33d152c425c78' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:9.0.0-noble-chiseled-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=9.0.0-rtm.24473.16

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
