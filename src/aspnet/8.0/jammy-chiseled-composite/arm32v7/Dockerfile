ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=8.0.6  \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz \
    && dotnet_sha512='f7fccfe2b4bde2317efc4954884d780bb9486a5e55bdbb50dadd65eea59681abbe9fdb9565a5ff13935b21fddeda2b2efb380811dbe0974cb39335e04f94d494' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:8.0.6-jammy-chiseled-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.6 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.6

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
