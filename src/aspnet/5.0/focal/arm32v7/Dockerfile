ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/5.0.9/aspnetcore-runtime-5.0.9-linux-arm.tar.gz \
    && aspnetcore_sha512='ea8d15c3db26b11fe1941a15996532906a6eee3ee281ab9b3153cbac79d108f59979464741a3bfc5b7cf959a030e70cd2d3a2150f49c2334eb788f1778cd1b5b' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0.9-focal-arm32v7

ENV ASPNET_VERSION=5.0.9

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
