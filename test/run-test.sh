#!/usr/bin/env bash
set -e 	# Exit immediately upon failure
set -o pipefail  # Carry failures over pipes

docker_repo="microsoft/dotnet"
repo_root="$( cd "$( dirname "${BASH_SOURCE[0]}" )" && pwd )/.."

if [ -z "${DEBUGTEST}" ]; then
    optional_docker_run_args="--rm"
fi

pushd "${repo_root}" > /dev/null

# Loop through each sdk Dockerfile in the repo and test the sdk and runtime images.
for sdk_tag in $( find . -path './.*' -prune -o -path '*/debian/sdk/*/Dockerfile' -print0 | xargs -0 -n1 dirname | sed -e 's/.\///' -e 's/\/debian\/sdk\//-sdk-/' ); do
    full_sdk_tag="${docker_repo}:${sdk_tag}"

    base_tag="$( echo "${full_sdk_tag}" | sed -e 's/-sdk//' -e 's/-msbuild//' -e 's/-projectjson//' )"

    app_name="app$(date +%s)"
    app_dir="${repo_root}/.test-assets/${app_name}"
    mkdir -p "${app_dir}"

    echo "----- Testing ${full_sdk_tag} -----"
    docker run -t ${optional_docker_run_args} -v "${app_dir}:/${app_name}" -v "${repo_root}/test:/test" --name "sdk-test-${app_name}" --entrypoint /test/create-run-publish-app.sh "${full_sdk_tag}" "${app_name}" "${sdk_tag}"

    echo "----- Testing ${base_tag}-runtime with ${sdk_tag} app -----"
    docker run -t ${optional_docker_run_args} -v "${app_dir}:/${app_name}" --name "runtime-test-${app_name}" --entrypoint dotnet "${base_tag}-runtime" "/${app_name}/publish/framework-dependent/${app_name}.dll"

    echo "----- Testing ${base_tag}-runtime-deps with ${sdk_tag} app -----"
    docker run -t ${optional_docker_run_args} -v "${app_dir}:/${app_name}" --name "runtime-deps-test-${app_name}" --entrypoint "/${app_name}/publish/self-contained/${app_name}" "${base_tag}-runtime-deps"
done

popd > /dev/null
