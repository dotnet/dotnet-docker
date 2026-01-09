ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.1 \
    && curl --fail --show-error --location \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz \
        --remote-name https://builds.dotnet.microsoft.com/dotnet/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz.sha512 \
    && sha512sum -c aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz.sha512 \
    && mkdir --parents /usr/share/dotnet \
    && tar --gzip --extract --no-same-owner --file aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz --directory /usr/share/dotnet \
    && rm \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz \
        aspnetcore-runtime-composite-$aspnetcore_version-linux-arm.tar.gz.sha512

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet


# ASP.NET Composite Image
FROM $REPO:10.0.1-noble-chiseled-extra-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.1 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.1

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]
