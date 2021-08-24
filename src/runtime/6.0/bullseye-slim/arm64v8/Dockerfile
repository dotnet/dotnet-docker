ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21423.9/dotnet-runtime-6.0.0-rc.1.21423.9-linux-arm64.tar.gz \
    && dotnet_sha512='00a203539c31fdb7948cfc63b5483cdc31d25c0820602bdc650068d7b4958c4a76586803e4192e28d70427f20c8e42744f290596d36cdab8676ee4e7df4d2414' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-arm64v8

ENV DOTNET_VERSION=6.0.0-rc.1.21423.9

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
