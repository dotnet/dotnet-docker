ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=9.0.0-preview.3.24172.13  \
    && curl -fSL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz \
    && dotnet_sha512='a6958b10bb735875670bb280b6187f963b65fa2a02f49848096b2a6c06526a39accefbf362394d6cb82d5cce65eb1819762365c7114cd7b7748908f814fdebca' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:9.0.0-preview.3-jammy-chiseled-extra-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=9.0.0-preview.3.24172.9 \
    # ASP.NET Core version
    ASPNET_VERSION=9.0.0-preview.3.24172.13

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
