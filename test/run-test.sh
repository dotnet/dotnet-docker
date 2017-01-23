#!/usr/bin/env bash
set -e # Exit immediately upon failure
set -o pipefail # Carry failures over pipes

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

    dotnetNewParam=""
    echo "DEBUG sdk_tag: $sdk_tag"
    if [[ $sdk_tag == "1.1-sdk-msbuild" ]]; then
        echo "DEBUG setting custom console stuff"
        dotnetNewParam="-t Console1.1"
    fi

    buildImage="build-image-${sdk_tag}"
    echo "----- Testing, create, restore and build for ${full_sdk_tag} -----"
    cat test/Dockerfile.test | sed -e "s#{image}#$full_sdk_tag#g" -e "s/{dotnetNewParam}/$dotnetNewParam/g" | docker build --rm -t ${buildImage} -

    echo "----- Testing running ${full_sdk_tag} -----"
    docker run $optional_docker_run_args -t ${optional_docker_run_args} ${buildImage} dotnet run

    echo "----- Testing publishing framework dependent app built on ${full_sdk_tag} -----"
    frameworkDepVol="framework-dep-publish-$app_name"
    docker run $optional_docker_run_args -v ${frameworkDepVol}:/volume $buildImage dotnet publish -o /volume
    echo "----- Testing ${base_tag}-runtime with ${sdk_tag} app -----"
    docker run $optional_docker_run_args -v ${frameworkDepVol}:/volume --entrypoint dotnet "${base_tag}-runtime" /volume/test.dll

    if [[ $sdk_tag == *"projectjson"* ]]; then
        projectType="projectjson"
        publishArg=""
    else
        projectType="msbuild"
        publishArg="-r debian.8-x64"
    fi

    selfContainedImage="self-contained-build-${buildImage}"
    selfContainedVol="self-contained-publish-$app_name"
    echo "----- Testing publishing self-contained app with ${full_sdk_tag} built on $buildImage -----"
    cat test/Dockerfile.linux.${projectType}.publish | sed -e "s#{image}#$buildImage#g" | docker build -t $selfContainedImage -
    docker run $optional_docker_run_args -v ${selfContainedVol}:/volume $selfContainedImage dotnet publish $publishArg -o /volume

    echo "----- Testing ${base_tag}-runtime with ${sdk_tag} app -----"
    docker run ${optional_docker_run_args} -v ${selfContainedVol}:/volume --entrypoint dotnet "${base_tag}-runtime" /volume/test.dll

    echo "----- Testing ${base_tag}-runtime-deps with ${sdk_tag} app -----"
    docker run ${optional_docker_run_args} -v ${selfContainedVol}:/volume --entrypoint "/volume/test" "${base_tag}-runtime-deps"
done

popd > /dev/null
