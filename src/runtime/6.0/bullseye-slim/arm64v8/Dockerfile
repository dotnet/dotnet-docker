ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21517.2/dotnet-runtime-6.0.0-rtm.21517.2-linux-arm64.tar.gz \
    && dotnet_sha512='798f88a5b42762b45c7006f576f9526cfd0a7e146938ad86e6af2e3e4630fb39e9abcd161040297dfc766f6a4e0966b0759a2ac139bba1f2f880c08e9c34aa16' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-bullseye-slim-arm64v8

ENV DOTNET_VERSION=6.0.0-rtm.21517.2

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
