ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Core
RUN aspnetcore_version=7.0.4 \
    && curl -fSL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-$aspnetcore_version-linux-x64.tar.gz \
    && aspnetcore_sha512='b0d2896928c003abf79c539c1c6be13ad560a34d8fdbe9438d916a977aa59e648d0737b57aafb25fda1c3de7c95997eccbea28ae04e4131ebfcd18c36940bcb4' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf aspnetcore.tar.gz -C /dotnet ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:7.0.4-jammy-chiseled-amd64

# ASP.NET Core version
ENV ASPNET_VERSION=7.0.4

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
