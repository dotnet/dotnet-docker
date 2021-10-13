ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21512.8/dotnet-runtime-6.0.0-rtm.21512.8-linux-x64.tar.gz \
    && dotnet_sha512='fd89c6baaa9f4c92ec64ba06dc5dacba50cf202fd9039aa3ef68961e35b98caca2f821444ed0194c4a8e746a2b7e6435b3616047cda86ccb71782db2578f2056' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-bullseye-slim-amd64

ENV DOTNET_VERSION=6.0.0-rtm.21512.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
