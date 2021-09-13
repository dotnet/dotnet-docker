ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.1.21451.13/dotnet-runtime-6.0.0-rc.1.21451.13-linux-x64.tar.gz \
    && dotnet_sha512='6c101a93766747deddc97dd3ae9cbd0d0babc292dff2897006e1df8ad6af100aa74063d4961d73e67c82db66662f760cf83f510617559a2538210fc89694d9ea' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.1-bullseye-slim-amd64

ENV DOTNET_VERSION=6.0.0-rc.1.21451.13

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
