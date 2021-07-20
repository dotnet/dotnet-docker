ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21369.8/dotnet-runtime-6.0.0-preview.7.21369.8-linux-arm64.tar.gz \
    && dotnet_sha512='2eafc10222893da982a973c1b5bf29d729d4f126719f257d638b4023d01479ec39d08b0ff63df9cd4dadf5a506aac3b32446a8e9613372f05b4f921cddca6b6c' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-preview.7.21369.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
