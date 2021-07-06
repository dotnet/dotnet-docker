ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM arm32v7/buildpack-deps:buster-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/5.0.7/dotnet-runtime-5.0.7-linux-arm.tar.gz \
    && dotnet_sha512='9bda7ae3cd708ab5b9f3acfa502ff534edc01ed2b77e97ab384df11125092e11aa4c6db143f8df4fb19ea5a4e7d0024579362e1c14bbb6f422d766e43e99cb72' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:5.0.7-buster-slim-arm32v7

ENV DOTNET_VERSION=5.0.7

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
