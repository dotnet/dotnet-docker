#!/usr/bin/env bash
set -e 	# Exit immediately upon failure
set -o pipefail  # Carry failures over pipes

docker_repo="microsoft/dotnet"
repo_root="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )/.."

# Maps development image versions to the corresponding runtime image version
version_mappings=( "1.0.0-preview2 1.0" )

if [ -z "${DEBUGTEST}" ]; then
    optional_docker_run_args="--rm"
fi

function get_runtime_image_version {
    for version_mapping in "${version_mappings[@]}"; do
        read development_image_version runtime_image_version <<< "${version_mapping}"
        if [ "${development_image_version}" == "${1}" ]; then
            echo "${runtime_image_version}"
            break
        fi
    done
}

pushd "${repo_root}" > /dev/null

# Loop through each sdk Dockerfile in the repo.  If it has an entry in $version_mappings, then test the sdk, core, and onbuild images; if not, fail.
for development_image_version in $( find . -path './.*' -prune -o -path '*/debian/Dockerfile' -print0 | xargs -0 -n1 dirname | sed -e 's/.\///' -e 's/\/debian//' ); do
    runtime_image_version="$( get_runtime_image_version "${development_image_version}" )"
    if [ -z "${runtime_image_version}" ]; then
        runtime_image_version="${development_image_version}"
    fi

    development_tag_base="${docker_repo}:${development_image_version}"
    runtime_tag_base="${docker_repo}:${runtime_image_version}"

    app_name="app$(date +%s)"
    app_dir="${repo_root}/.test-assets/${app_name}"
    mkdir -p "${app_dir}"

    echo "----- Testing ${development_tag_base}-sdk -----"
    docker run -t ${optional_docker_run_args} -v "${app_dir}:/${app_name}" -v "${repo_root}/test:/test" --name "sdk-test-${app_name}" --entrypoint /test/create-run-publish-app.sh "${development_tag_base}-sdk" "${app_name}"

    echo "----- Testing ${runtime_tag_base}-core -----"
    docker run -t ${optional_docker_run_args} -v "${app_dir}:/${app_name}" --name "core-test-${app_name}" --entrypoint dotnet "${runtime_tag_base}-core" "/${app_name}/publish/${app_name}.dll"

    echo "----- Testing ${development_tag_base}-onbuild -----"
    pushd "${app_dir}" > /dev/null
    echo "FROM ${development_tag_base}-onbuild" > Dockerfile
    docker build -t "${app_name}-onbuild" .
    popd > /dev/null
    docker run -t ${optional_docker_run_args} --name "onbuild-test-${app_name}" "${app_name}-onbuild"

    if [ -z "${DEBUGTEST}" ]; then
        docker rmi "${app_name}-onbuild"
    fi
done

popd > /dev/null
