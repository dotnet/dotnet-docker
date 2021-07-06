ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.7/aspnetcore-runtime-5.0.7-linux-x64.tar.gz \
    && aspnetcore_sha512='03399412dfa6ec9b75125b38d7d4297e0cb8cb4ae37cfb27d6cc948942763691c61647d83c3a8a068291a389c35fc6a89d375c97253335c6a8985d55fcac8918' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.7-focal-amd64

ENV ASPNET_VERSION=5.0.7

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
