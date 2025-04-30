ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm32v7 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.4.25229.110 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-arm.tar.gz \
    && dotnet_sha512='3bd744513eb0253173abb10574a3a79d647e956ea8b1fe7044dded56693d0770ceec58f1e21f2f0d5eb337ff74264135ff93dc366bd5e7fd4ebd26482f90b8e2' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.3-alpine3.21-arm32v7

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.4.25229.110

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
