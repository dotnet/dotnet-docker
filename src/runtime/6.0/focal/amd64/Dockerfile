ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21514.7/dotnet-runtime-6.0.0-rtm.21514.7-linux-x64.tar.gz \
    && dotnet_sha512='053c74c3b24b416981863a6967bcde71bf15300572b13e5aafb7839e39f47bf641807d1c684387ec47b602d9a238d072fd08e329802f43baa6dc651f74fb3226' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-focal-amd64

ENV DOTNET_VERSION=6.0.0-rtm.21514.7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
