ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:buster-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=5.0.15 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='df1037918f652b68d84639718ece0a726a6caeb977e125a3a57fe8fe146af43a892335ccf193efc1926962e4b1d8e05b8b7514303ede574478fe3e1c2f8f0d68' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -oxzf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.15-buster-slim-arm64v8

# ASP.NET Core version
ENV ASPNET_VERSION=5.0.15

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
