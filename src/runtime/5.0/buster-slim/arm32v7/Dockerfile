ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:buster-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.9/dotnet-runtime-5.0.9-linux-arm.tar.gz \
    && dotnet_sha512='9019858e75f73b5fd6ce0e04aeb060f61221391b6a55dbf8ed2e43f1011198252f651fb0e07b2b8e7d8d558403f2c86f6ced147c2501e36a90411bd21e521a4f' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.9-buster-slim-arm32v7

ENV DOTNET_VERSION=5.0.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
