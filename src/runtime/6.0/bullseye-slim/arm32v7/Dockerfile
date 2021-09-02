ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21451.13/dotnet-runtime-6.0.0-rc.1.21451.13-linux-arm.tar.gz \
    && dotnet_sha512='6a5786fb0fc10cbe562350c56c35fb4745db18dbf3b299e3d86bd0c0e1bd43b95cd54561d78f1d53f3c41639a4505527e8fc0cf37e4f9da469ac0f521cec279b' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-rc.1.21451.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
