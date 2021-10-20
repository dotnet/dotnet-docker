ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21518.12/dotnet-runtime-6.0.0-rtm.21518.12-linux-arm64.tar.gz \
    && dotnet_sha512='c562591d39260a2611e1a9e93c6fd131f4fb2b05573701cb70cfd949e028a53411a9d6e073ccaa285577ff514bbb93fbbf1be072f54e633b8ed3c4b3ee4120fe' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-bullseye-slim-arm64v8

ENV DOTNET_VERSION=6.0.0-rtm.21518.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
