ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:bullseye-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rtm.21518.12/dotnet-runtime-6.0.0-rtm.21518.12-linux-x64.tar.gz \
    && dotnet_sha512='7384dcd19df4c0d9cc9154c4f934e1cacd0a8ead72fa833b31ace2d074c90d8af83831ba5aa6e6865e963d300d5452b2abf511f7e0d508b4002f2975365de0e7' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-bullseye-slim-amd64

ENV DOTNET_VERSION=6.0.0-rtm.21518.12

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
