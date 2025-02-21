ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:9.0.2-alpine3.21-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=9.0.2 \
    && wget -O dotnet.tar.gz https://builds.dotnet.microsoft.com/dotnet/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='38e03d8c12fa4520e311cf2d15fb2c2f0e019c7165b13fcbe58fc46914743ecea8e0ac4d914385b1430cfb5e0df3db6299424cfdccea6ffe469a681e2d9d93b7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:9.0.2-alpine3.21-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=9.0.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
