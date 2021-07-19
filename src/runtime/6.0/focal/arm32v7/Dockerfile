ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.6.21352.12/dotnet-runtime-6.0.0-preview.6.21352.12-linux-arm.tar.gz \
    && dotnet_sha512='7983d25361d7acf405ea2296e94231c3f313791d775fb44aa7b8455c17a5908a983ba43a0a9a67daf83a9fdccfa87f6bce6b931bbb43bb6896959ac00819ac36' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.6-focal-arm32v7

ENV DOTNET_VERSION=6.0.0-preview.6.21352.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
