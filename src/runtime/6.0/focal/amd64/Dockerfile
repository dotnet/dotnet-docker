ARG REPO=mcr.microsoft.com/dotnet/runtime-deps

# Installer image
FROM amd64/buildpack-deps:focal-curl as installer

# Retrieve .NET
RUN curl -SL --output dotnet.tar.gz https://dotnetcli.azureedge.net/dotnet/Runtime/6.0.0-rc.2.21468.3/dotnet-runtime-6.0.0-rc.2.21468.3-linux-x64.tar.gz \
    && dotnet_sha512='14b7a6d46157bc456a8c141bba57a67e727fa44ce19ef3ec64b315e986c8933bee1492b21dc5c60f37a7b2e252d0503367da08d24e4c013de7f56053020eb9c4' \
    && echo "$dotnet_sha512  dotnet.tar.gz" | sha512sum -c - \
    && mkdir -p /dotnet \
    && tar -ozxf dotnet.tar.gz -C /dotnet \
    && rm dotnet.tar.gz


# .NET runtime image
FROM $REPO:6.0.0-rc.2-focal-amd64

ENV DOTNET_VERSION=6.0.0-rc.2.21468.3

COPY --from=installer ["/dotnet", "/usr/share/dotnet"]

RUN ln -s /usr/share/dotnet/dotnet /usr/bin/dotnet
