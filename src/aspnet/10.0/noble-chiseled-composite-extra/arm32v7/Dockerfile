ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.4.25255.103  \
    && curl -fSL --output dotnet.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz \
    && dotnet_sha512='72adf8a8fe8d17987bae45be8ce4dd14c7cccca2a52a146d86e5603a2e3115565290d71bc6a5b1e8490569e74185b01d7007c6d8be089da50fc1066a201292e8' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.3-noble-chiseled-extra-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.4.25255.103 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.4.25255.103

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
