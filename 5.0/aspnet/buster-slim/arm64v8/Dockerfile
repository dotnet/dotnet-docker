ARG REPO=mcr.microsoft.com/dotnet/core/runtime

# Installer image
FROM arm64v8/buildpack-deps:buster-curl as installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=5.0.0-preview.2.20155.1 \
    && curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-arm64.tar.gz \
    && aspnetcore_sha512='b93728eaa266790c577bd7b0dada9313725eccaaa43a505cf38458288a03d3fc08c327ff40f009cfcaccafa85e388827b962589b7db2072655b1ec502e0fac77' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-buster-slim-arm64v8

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
