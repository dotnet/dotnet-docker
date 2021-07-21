ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21370.18/dotnet-runtime-6.0.0-preview.7.21370.18-linux-arm64.tar.gz \
    && dotnet_sha512='42e799a7721132449308c48a1598708e2bbc5a4ff89a65e7e0b4c9f4965aeeee3cdb447240aa6a8abce9d29e0db84c1ad09fb2437a50d1688556f5146e295e05' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-preview.7.21370.18

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
