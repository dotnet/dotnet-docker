ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21425.15/dotnet-runtime-6.0.0-rc.1.21425.15-linux-arm.tar.gz \
    && dotnet_sha512='ab146287e0134943a551c49f4f1c9dfc69c26cf84ef5880841d6046f08cd2239883c3febbe7fcc2c57d89f3bd099e75cd9dc65eb2e7489ff033b1a820a00f595' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-arm32v7

ENV DOTNET_VERSION=6.0.0-rc.1.21425.15

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
