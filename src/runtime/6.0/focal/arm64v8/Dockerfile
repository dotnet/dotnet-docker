ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21377.12/dotnet-runtime-6.0.0-preview.7.21377.12-linux-arm64.tar.gz \
    && dotnet_sha512='f07ecbe954f71678c47e858671361e83022386dbd14b4cef19891375439a1a15df0a163824207910c229ef002d6897b50edeb9655c9bc3d402e66996c2afd027' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-focal-arm64v8

ENV DOTNET_VERSION=6.0.0-preview.7.21377.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
