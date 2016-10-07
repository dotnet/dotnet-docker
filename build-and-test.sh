#!/usr/bin/env bash
set -e 	# Exit immediately upon failure
set -o pipefail  # Carry failures over pipes

repo_root="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )"
docker_repo="microsoft/dotnet"

function build_dockerfiles {
    for dockerfile_dir in $( egrep -v 'nanoserver' <<< "${1}" | sort ); do
        tag="${docker_repo}:$( sed -e 's/.\///' -e 's/debian\///' -e 's/debian/sdk/' -e 's/\//-/g' <<< "${dockerfile_dir}" )"
        echo "----- Building ${tag} -----"
        docker build --no-cache -t "${tag}" "${dockerfile_dir}"
    done
}

pushd "${repo_root}" > /dev/null

build_dockerfiles "$( find . -path './.*' -prune -o -name 'Dockerfile' -a -path '*/*-deps/*' -print0 | xargs -0 -n1 dirname )"

build_dockerfiles "$( find . -path './.*' -prune -o -name 'Dockerfile' -a ! -path '*/*-deps/*' -print0 | xargs -0 -n1 dirname)"

popd > /dev/null

"${repo_root}/test/run-test.sh"
