ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=9.0.0-preview.4.24213.1  \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz \
    && dotnet_sha512='1df010f6dc9546620af49a4e1b4db63c8b2d7001b2262d4213cb954d709a8b8cc100a79aff539105015b1e30bb2c93053c39200f49974b6166705689576dc006' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:9.0.0-preview.4-jammy-chiseled-extra-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-preview.4.24211.4 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-preview.4.24213.1

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
