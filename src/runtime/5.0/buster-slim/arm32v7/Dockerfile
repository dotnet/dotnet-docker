ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:buster-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.10/dotnet-runtime-5.0.10-linux-arm.tar.gz \
    && dotnet_sha512='3f0e86f14fe8a9b8773a6078fe236e746ca078cb1deab36f4b770170d6a4322b1c870328148934dd4bd5953d5962de4a32ef487c6142079759512b8281d8c7a1' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.10-buster-slim-arm32v7

ENV DOTNET_VERSION=5.0.10

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
