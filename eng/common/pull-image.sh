#!/usr/bin/env bash

# Stop script on NZEC
set -e
# Stop script if unbound variable found (use ${var:-} if intentional)
set -u

say_err() {
    printf "%b\n" "Error: $1" >&2
}

# Executes a command and retries if it fails.
execute() {
    local count=0
    until "$@"; do
        local exit=$?
        count=$(( $count + 1 ))
        if [ $count -lt $retries ]; then
            local wait=$(( waitFactor ** (( count - 1 )) ))
            echo "Retry $count/$retries exited $exit, retrying in $wait seconds..."
            sleep $wait
        else    
            say_err "Retry $count/$retries exited $exit, no more retries left."
            return $exit
        fi
    done

    return 0
}

scriptName=$0
retries=5
waitFactor=6
image=$1

echo "Pulling Docker image $image"
execute docker pull $image
