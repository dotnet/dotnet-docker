FROM amd64/alpine:3.17

RUN apk add --no-cache \
        ca-certificates \
        \
        # .NET dependencies
        krb5-libs \
        libgcc \
        libintl \
        libssl3 \
        libstdc++ \
        zlib

# Create a non-root user and group
# alpine linux guide on adding users: https://wiki.alpinelinux.org/wiki/Setting_up_a_new_user
RUN addgroup \
        --system \
        --gid 101 \
        app \
    && adduser \
        --uid 101 \
        --ingroup app \
        --shell /bin/false \
        --system \
        app
    # && install -d -m 0755 -o 101 -g 101 "/staging/home/app" \
    # && rootOrAppRegex='^\(root\|app\):' \
    # && cat /etc/passwd | grep $rootOrAppRegex > "/staging/etc/passwd" \
    # && cat /etc/group | grep $rootOrAppRegex > "/staging/etc/group"

ENV \
    # Configure web servers to bind to port 80 when present
    # ASPNETCORE_URLS=http://+:80 \
    ASPNETCORE_HTTP_PORTS=8080 \
    ASPNETCORE_HTTPS_PORTS=8443 \
    # ASPNETCORE_URLS=http://+:80 \
    # Enable detection of running in a container
    DOTNET_RUNNING_IN_CONTAINER=true \
    # Set the invariant mode since ICU package isn't included (see https://github.com/dotnet/announcements/issues/20)
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=true
