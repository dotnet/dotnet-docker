ARG REPO=mcr.microsoft.com/dotnet/core/runtime

# Installer image
FROM buildpack-deps:buster-curl as installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=5.0.0-preview.2.20155.1 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='62f12a2e9043e92fa161c7723ca47c603a1035af4fbdf75917d2157c805b0e1f0480101cc84079bac55d5eb21c363f9b86b5eb16a6ba9fc70e633dd9f9d58d99' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-buster-slim

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
