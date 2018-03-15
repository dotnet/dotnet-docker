FROM alpine:3.6

RUN apk add --no-cache \
        ca-certificates \
        \
        # .NET Core dependencies
        krb5-libs \
        libcurl \
        libgcc \
        libintl \
        libssl1.0 \
        libstdc++ \
        libunwind \
        tzdata \
        userspace-rcu \
        zlib \
    && apk -X https://dl-cdn.alpinelinux.org/alpine/edge/main add --no-cache \
        lttng-ust

# Set the invariant mode since icu_libs isn't included (see https://github.com/dotnet/announcements/issues/20)
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT true

# Enables checking on running in a container (required for some scenarios)
ENV DOTNET_RUNNING_IN_CONTAINER true

# Configure Kestrel web server to bind to port 80 when present
ENV ASPNETCORE_URLS=http://+:80
