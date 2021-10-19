ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21518.8/dotnet-runtime-6.0.0-rtm.21518.8-linux-x64.tar.gz \
    && dotnet_sha512='d0123a76598c28e6d88294c598921c78c11aad678e41f9891cbed3c301f4bc15801757d8c90183a99f8f14c6514078254f91c0d3654b23edb094b02a9acff56c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-focal-amd64

ENV DOTNET_VERSION=6.0.0-rtm.21518.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
