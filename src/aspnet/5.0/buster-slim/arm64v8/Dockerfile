ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:buster-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.8/aspnetcore-runtime-5.0.8-linux-arm64.tar.gz \
    && aspnetcore_sha512='966abfdddd43068fef72cdf3deaf49417a092437fa0ee829b6b3ced23a32246bf3bd0b85feb470fe6c326d54b9a2bee93cacffe603640faa0b935a3cde9e0760' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.8-buster-slim-arm64v8

ENV ASPNET_VERSION=5.0.8

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
