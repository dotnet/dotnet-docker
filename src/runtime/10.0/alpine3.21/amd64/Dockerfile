ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM $REPO:10.0.0-preview.2-alpine3.21-amd64 AS installer

# Retrieve .NET Runtime
RUN dotnet_version=10.0.0-preview.2.25163.2 \
    && wget -O dotnet.tar.gz https://ci.dot.net/public/Runtime/$dotnet_version/dotnet-runtime-$dotnet_version-linux-musl-x64.tar.gz \
    && dotnet_sha512='d91bfb5839b30377d09849511c8668230be51fae0d05e07d761297da40c994d800de5ce6fc5f7307cbdf5cf6e7fa7dd720ecab8354ec9436c662cd4fe14851dd' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -oxzf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:10.0.0-preview.2-alpine3.21-amd64

# .NET Runtime version
ENV DOTNET_VERSION=10.0.0-preview.2.25163.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
