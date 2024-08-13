ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=6.0.33 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='12b34fe1d0a679ff63db4bf002a2988194d9e64d0e107d128c247821dd939a86eced0fe453c0638d3742dac3a32e533792c26299400fb4fd5566b75177e66875' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.33-jammy-chiseled-extra-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=6.0.33

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
