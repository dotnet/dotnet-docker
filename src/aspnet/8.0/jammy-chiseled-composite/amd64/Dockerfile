ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:jammy-curl AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=8.0.0-preview.5.23302.2  \
    && curl -fSL --output dotnet.tar.gz https://dotnetbuilds.azureedge.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-x64.tar.gz \
    && dotnet_sha512='42e801b252a1f41e60bb795d029f863f154d3a8d21027d53d5bc3811b394dca3bfb36887feecbfccc9e143c58cb13cf8b8dd4e1c46cb24ae1191dff7c1566793' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /usr/share/dotnet \
    && tar -oxzf dotnet.tar.gz -C /usr/share/dotnet \
    && rm dotnet.tar.gz

RUN mkdir /dotnet-symlink \
    && ln -s /usr/share/dotnet/dotnet /dotnet-symlink/dotnet

# .NET Runtime-Deps Image
FROM $REPO:8.0.0-preview.5-jammy-chiseled-amd64

ENV \
    # .NET Runtime version
    DOTNET_VERSION=8.0.0-preview.5.23280.8 \
    # ASP.NET Core version
    ASPNET_VERSION=8.0.0-preview.5.23302.2

COPY --from=installer ["/usr/share/dotnet", "/usr/share/dotnet"]
COPY --from=installer ["/dotnet-symlink", "/usr/bin"]

ENTRYPOINT ["/usr/bin/dotnet"]
CMD ["--info"]

