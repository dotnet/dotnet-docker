ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.6-alpine3.22-arm32v7 AS installer

# Retrieve ASP.NET Composite Runtime
RUN aspnetcore_version=10.0.0-preview.6.25326.107 \
    && wget --output-document dotnet.tar.gz https://ci.dot.net/public/aspnetcore/Runtime/$aspnetcore_version/aspnetcore-runtime-composite-$aspnetcore_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='418660bd641b3aeeedc254f1de00f189ea0cbb765ee98e8488dfd422e60566df463714c0acb4d4b03202b45e4d6e8de538a5b89e58e26cad93a7cd90390768e2' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir --parents /dotnet \
    && tar --gzip --extract --no-same-owner --file dotnet.tar.gz --directory /dotnet \
    && rm dotnet.tar.gz


# ASP.NET Composite Image
FROM $REPO:10.0.0-preview.6-alpine3.22-arm32v7

ENV \
    # .NET Runtime version
    DOTNET_VERSION=10.0.0-preview.6.25326.107 \
    # ASP.NET Core version
    ASPNET_VERSION=10.0.0-preview.6.25326.107

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]
RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
