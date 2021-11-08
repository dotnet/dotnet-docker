ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.12/aspnetcore-runtime-5.0.12-linux-arm64.tar.gz \
    && aspnetcore_sha512='70570177896943613f0cddeb046ffccaafb1c8245c146383e45fbcfb27779c70dff1ab22c2b13a14bf096173c9279e0a386f61665106a3abb5f623b50281a652' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.12-focal-arm64v8

ENV ASPNET_VERSION=5.0.12

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
