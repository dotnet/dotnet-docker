ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21518.8/dotnet-runtime-6.0.0-rtm.21518.8-linux-arm.tar.gz \
    && dotnet_sha512='3a99cdd0db344b2956b7cd032e9523dede8b3aed4082acf1896cadb9571449d46aa9476157c8fbad8a2ef60ebec43a9446014d76cb9d6f87e86e51e9d1b4a11a' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-rtm.21518.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
