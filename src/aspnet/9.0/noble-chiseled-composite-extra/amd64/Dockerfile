ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:noble-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=9.0.0-preview.6.24277.5  \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz \
    && dotnet_sha512='f6389b3d65dce369f43f55f3c9f4ad8cef1847aaf8641545754472ef31951cc969f42d56736df968980a68e43215668da277713f122fedaa0f949a8903548bef' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:9.0.0-preview.5-noble-chiseled-extra-amd64

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-preview.5.24272.2 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-preview.6.24277.5

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
