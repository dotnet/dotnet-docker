ARG REPO=mcr.microsoft.com/dotnet/runtime
ARG ASPNET_VERSION=5.0.0-rtm.20520.6

# Installer image
FROM arm64v8/buildpack-deps:buster-curl as installer
ARG ASPNET_VERSION

# Retrieve ASP.NET Core
RUN curl -SL --output aspnetcore.tar.gz https://dotnetcli.azureedge.net/dotnet/aspnetcore/Runtime/$ASPNET_VERSION/aspnetcore-runtime-5.0.0-linux-arm64.tar.gz \
    && aspnetcore_sha512='6cbfce46197a2be8182fd0a0ad7c32d4ff5acdda0b2e76b0fd2874106ae576fa13c311f83a3f25bbe75a789dfa8d440902c8ec1f4edc139cdca244d43f9312e7' \
    && echo "$aspnetcore_sha512  aspnetcore.tar.gz" | sha512sum -c - \
    && tar -ozxf aspnetcore.tar.gz ./shared/Microsoft.AspNetCore.App \
    && rm aspnetcore.tar.gz


# ASP.NET Core image
FROM $REPO:5.0-buster-slim-arm64v8
ARG ASPNET_VERSION

ENV ASPNET_VERSION $ASPNET_VERSION

COPY --from=installer ["/shared/Microsoft.AspNetCore.App", "/usr/share/dotnet/shared/Microsoft.AspNetCore.App"]
