ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-preview.7.21369.8/dotnet-runtime-6.0.0-preview.7.21369.8-linux-arm.tar.gz \
    && dotnet_sha512='e9ff425057937686b7db2d3058b219cef4dabf34de2a2b2f81ae9cb01638bc943e39772eb91c37de4392481b3ec56598e04cda3f701fc64bfe5b4cf4d93474a7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-preview.7-focal-arm32v7

ENV DOTNET_VERSION=6.0.0-preview.7.21369.8

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
