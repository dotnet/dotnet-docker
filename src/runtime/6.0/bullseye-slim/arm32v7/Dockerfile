ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21512.8/dotnet-runtime-6.0.0-rtm.21512.8-linux-arm.tar.gz \
    && dotnet_sha512='56b2585dfac2758df60f3604fc11e9ce2639b591327e3eb7db0c356b46b5799d9783f915adf2cc5dc745eff6e5f229c41febf9addde70a627660097871aa3e4c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-rtm.21512.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
