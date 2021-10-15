ARG REPO=mcr.microsoft.com/dotnet/runtime

# Installer image
FROM arm64v8/buildpack-deps:focal-curl as installer

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/6.0.0-rtm.21514.23/aspnetcore-runtime-6.0.0-rtm.21514.23-linux-arm64.tar.gz \
    && aspnetcore_sha512='27761765818a5f14043a086bac90612e49b721fdcbbfe19db06e4664e635403d920b201d8f1a7087657ed4ad569ec2cf891c90c07774904519cf512afdb9ba5a' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:6.0.0-focal-arm64v8

ENV \
    # ASP.NET Core version
    ASPNET_VERSION=6.0.0-rtm.21514.23 \
    # Set the default console formatter to JSON
    Logging__Console__FormatterName=Json

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
