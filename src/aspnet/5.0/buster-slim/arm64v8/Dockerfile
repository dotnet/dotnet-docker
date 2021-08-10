ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:buster-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.9/aspnetcore-runtime-5.0.9-linux-arm64.tar.gz \
    && aspnetcore_sha512='7bad5fa5fb8dc7eba9babc06d68b170e127df8d5729adf26530b2858aa4f411ca35c4641da0b0b13c7340fc5843e3b1d70b2038eabfca64d598e06fcf1b47edb' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.9-buster-slim-arm64v8

ENV ASPNET_VERSION=5.0.9

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
