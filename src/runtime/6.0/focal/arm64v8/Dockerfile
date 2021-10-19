ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21518.8/dotnet-runtime-6.0.0-rtm.21518.8-linux-arm64.tar.gz \
    && dotnet_sha512='7a727dba7894402f4e8febc04e05ed33310bebc3ee0f983d2edf1dfd1fee0d53b5ace4b2eb4351e4ddbeb2f355db7280d24d5862a2e909caa1828f0c757216ac' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-rtm.21518.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
