pipeline {
    agent {
        label 'dotnet-docker-agent'
    }

    environment {
        LOCAL_IMAGE_NAME        = 'aspnetapp-ci'
        HOST_PORT               = '18080'

        AWS_REGION              = 'ap-south-1'
        AWS_PAGER               = ''
        EXPECTED_AWS_ACCOUNT_ID = '800960612118'
        ECR_REPOSITORY          = 'dotnet-aspnetapp'
    }

    options {
        skipDefaultCheckout(true)
        timestamps()
        disableConcurrentBuilds()
        timeout(time: 40, unit: 'MINUTES')

        buildDiscarder(
            logRotator(
                numToKeepStr: '20',
                daysToKeepStr: '14'
            )
        )
    }

    stages {
        stage('Clean Workspace') {
            steps {
                deleteDir()
            }
        }

        stage('Checkout Source') {
            steps {
                script {
                    def scmVariables = checkout scm

                    env.FULL_GIT_COMMIT =
                        scmVariables.GIT_COMMIT ?: sh(
                            label: 'Read full Git commit',
                            returnStdout: true,
                            script: '''#!/usr/bin/env bash
set -Eeuo pipefail

git rev-parse HEAD
'''
                        ).trim()

                    env.GIT_REMOTE_URL =
                        scmVariables.GIT_URL ?: sh(
                            label: 'Read Git remote URL',
                            returnStdout: true,
                            script: '''#!/usr/bin/env bash
set -Eeuo pipefail

git config --get remote.origin.url
'''
                        ).trim()
                }
            }
        }

        stage('Initialize Build') {
            steps {
                script {
                    env.SHORT_GIT_COMMIT = sh(
                        label: 'Calculate short Git commit',
                        returnStdout: true,
                        script: '''#!/usr/bin/env bash
set -Eeuo pipefail

git rev-parse --short=12 HEAD
'''
                    ).trim()

                    env.IMAGE_TAG =
                        "sha-${env.SHORT_GIT_COMMIT}"

                    env.CONTAINER_NAME =
                        "aspnetapp-${env.BUILD_NUMBER}-${env.SHORT_GIT_COMMIT}"
                }

                echo "Jenkins node: ${NODE_NAME}"
                echo "Branch: ${BRANCH_NAME}"
                echo "Git commit: ${FULL_GIT_COMMIT}"
                echo "Git repository: ${GIT_REMOTE_URL}"
                echo "Local image: ${LOCAL_IMAGE_NAME}:${IMAGE_TAG}"
                echo "Test container: ${CONTAINER_NAME}"
                echo "Test URL: http://127.0.0.1:${HOST_PORT}"
            }
        }

        stage('Verify Docker') {
            steps {
                sh(
                    label: 'Verify Docker environment',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

echo "Jenkins node:"
echo "${NODE_NAME}"

echo
echo "Build user:"
whoami

echo
echo "Build hostname:"
hostname

echo
echo "Build user groups:"
id

echo
echo "Docker executable:"
command -v docker

echo
echo "Docker client and server:"
docker version

echo
echo "Docker Buildx:"
docker buildx version

echo
echo "Docker daemon:"
docker info \
  --format 'Server={{.ServerVersion}} Driver={{.Driver}} Logging={{.LoggingDriver}}'
'''
                )
            }
        }

        stage('Validate Repository') {
            steps {
                sh(
                    label: 'Validate ASP.NET project files',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

test -f Jenkinsfile
test -d samples/aspnetapp
test -f samples/aspnetapp/Dockerfile
test -d samples/aspnetapp/aspnetapp
test -f samples/aspnetapp/aspnetapp/aspnetapp.csproj
test -f samples/aspnetapp/aspnetapp/Program.cs

echo "ASP.NET application directory:"
ls -la samples/aspnetapp

echo
echo "ASP.NET source directory:"
ls -la samples/aspnetapp/aspnetapp

echo
echo "Dockerfile:"
sed -n '1,200p' samples/aspnetapp/Dockerfile

echo
echo "Repository validation completed successfully."
'''
                )
            }
        }

        stage('Build Container Image') {
            steps {
                sh(
                    label: 'Build ASP.NET container image',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

docker build \
  --pull \
  --label "org.opencontainers.image.revision=${FULL_GIT_COMMIT}" \
  --label "org.opencontainers.image.source=${GIT_REMOTE_URL}" \
  --label "org.opencontainers.image.created=$(date -u +%Y-%m-%dT%H:%M:%SZ)" \
  --tag "${LOCAL_IMAGE_NAME}:${IMAGE_TAG}" \
  samples/aspnetapp

echo
echo "Created image:"

docker image ls \
  --filter "reference=${LOCAL_IMAGE_NAME}:${IMAGE_TAG}"
'''
                )
            }
        }

        stage('Inspect Image') {
            steps {
                sh(
                    label: 'Inspect image metadata and runtime user',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

full_image_name="${LOCAL_IMAGE_NAME}:${IMAGE_TAG}"

echo "Inspecting image:"
echo "${full_image_name}"

echo
echo "Image ID:"

docker image inspect \
  --format '{{.Id}}' \
  "${full_image_name}"

echo
echo "Configured runtime user:"

image_user="$(
  docker image inspect \
    --format '{{.Config.User}}' \
    "${full_image_name}"
)"

echo "${image_user}"

test -n "${image_user}"
test "${image_user}" != "0"
test "${image_user}" != "root"

echo
echo "Configured exposed ports:"

docker image inspect \
  --format '{{json .Config.ExposedPorts}}' \
  "${full_image_name}"

echo
echo "Image size:"

image_size="$(
  docker image inspect \
    --format '{{.Size}}' \
    "${full_image_name}"
)"

echo "${image_size} bytes"

test "${image_size}" -gt 0

echo
echo "Image labels:"

docker image inspect \
  --format '{{json .Config.Labels}}' \
  "${full_image_name}" |
  jq .

echo
echo "Image successfully uses a non-root runtime user."
'''
                )
            }
        }

        stage('Start Container') {
            steps {
                sh(
                    label: 'Start isolated test container',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

docker rm \
  --force \
  "${CONTAINER_NAME}" \
  >/dev/null 2>&1 || true

docker run \
  --detach \
  --name "${CONTAINER_NAME}" \
  --publish "127.0.0.1:${HOST_PORT}:8080" \
  --env ASPNETCORE_HTTP_PORTS=8080 \
  --cap-drop ALL \
  --security-opt no-new-privileges:true \
  --memory 512m \
  --cpus 1 \
  --pids-limit 200 \
  "${LOCAL_IMAGE_NAME}:${IMAGE_TAG}"

echo
echo "Running container:"

docker ps \
  --filter "name=^/${CONTAINER_NAME}$"

echo
echo "Container port mapping:"

docker port "${CONTAINER_NAME}"
'''
                )
            }
        }

        stage('Container Health Check') {
            steps {
                sh(
                    label: 'Wait for ASP.NET health endpoint',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

health_url="http://127.0.0.1:${HOST_PORT}/healthz"
application_ready=false

for attempt in $(seq 1 30); do
  echo "Health-check attempt ${attempt}/30"

  if ! docker ps \
    --format '{{.Names}}' |
    grep -Fxq "${CONTAINER_NAME}"; then

    echo "Container stopped before becoming ready."

    docker ps \
      --all \
      --filter "name=^/${CONTAINER_NAME}$"

    docker logs "${CONTAINER_NAME}" || true

    exit 1
  fi

  if curl \
    --fail \
    --silent \
    --show-error \
    "${health_url}" \
    >/dev/null; then

    application_ready=true
    break
  fi

  sleep 2
done

if [[ "${application_ready}" != "true" ]]; then
  echo "Application health endpoint did not become ready."

  docker ps \
    --all \
    --filter "name=^/${CONTAINER_NAME}$"

  docker logs "${CONTAINER_NAME}" || true

  exit 1
fi

echo
echo "ASP.NET health endpoint is ready:"
echo "${health_url}"
'''
                )
            }
        }

        stage('Container Smoke Test') {
            steps {
                sh(
                    label: 'Verify ASP.NET Environment endpoint',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

response_file="${WORKSPACE}/environment-response.json"
application_url="http://127.0.0.1:${HOST_PORT}/Environment"

curl \
  --fail \
  --silent \
  --show-error \
  --retry 3 \
  --retry-delay 2 \
  "${application_url}" \
  --output "${response_file}"

echo "Application response:"

jq . "${response_file}"

jq -e '
  .runtimeVersion != null and
  .runtimeVersion != "" and
  .osVersion != null and
  .osArchitecture != null and
  .user != null and
  .user != "" and
  .hostName != null and
  .processorCount != null
' "${response_file}" >/dev/null

echo
echo "ASP.NET Environment endpoint validation succeeded."
'''
                )
            }
        }

        stage('Runtime Security Check') {
            steps {
                sh(
                    label: 'Verify container runtime security',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

response_file="${WORKSPACE}/environment-response.json"

runtime_user="$(
  jq -r '.user' "${response_file}"
)"

container_running="$(
  docker inspect \
    --format '{{.State.Running}}' \
    "${CONTAINER_NAME}"
)"

container_status="$(
  docker inspect \
    --format '{{.State.Status}}' \
    "${CONTAINER_NAME}"
)"

no_new_privileges="$(
  docker inspect \
    --format '{{json .HostConfig.SecurityOpt}}' \
    "${CONTAINER_NAME}"
)"

dropped_capabilities="$(
  docker inspect \
    --format '{{json .HostConfig.CapDrop}}' \
    "${CONTAINER_NAME}"
)"

port_binding="$(
  docker inspect \
    --format '{{json .HostConfig.PortBindings}}' \
    "${CONTAINER_NAME}"
)"

memory_limit="$(
  docker inspect \
    --format '{{.HostConfig.Memory}}' \
    "${CONTAINER_NAME}"
)"

nano_cpus="$(
  docker inspect \
    --format '{{.HostConfig.NanoCpus}}' \
    "${CONTAINER_NAME}"
)"

pid_limit="$(
  docker inspect \
    --format '{{.HostConfig.PidsLimit}}' \
    "${CONTAINER_NAME}"
)"

echo "Container running: ${container_running}"
echo "Container status: ${container_status}"
echo "Runtime user: ${runtime_user}"
echo "Security options: ${no_new_privileges}"
echo "Dropped capabilities: ${dropped_capabilities}"
echo "Port bindings: ${port_binding}"
echo "Memory limit: ${memory_limit}"
echo "Nano CPUs: ${nano_cpus}"
echo "PID limit: ${pid_limit}"

test "${container_running}" = "true"
test "${container_status}" = "running"

test -n "${runtime_user}"
test "${runtime_user}" != "root"
test "${runtime_user}" != "0"

echo "${no_new_privileges}" |
  grep -q 'no-new-privileges'

echo "${dropped_capabilities}" |
  grep -q 'ALL'

echo "${port_binding}" |
  grep -q '"HostIp":"127.0.0.1"'

test "${memory_limit}" -gt 0
test "${nano_cpus}" -gt 0
test "${pid_limit}" -gt 0

echo
echo "Runtime security checks completed successfully."
'''
                )
            }
        }

        stage('Show Container Logs') {
            steps {
                sh(
                    label: 'Display successful container logs',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

echo "Container logs:"
docker logs "${CONTAINER_NAME}"
'''
                )
            }
        }

        stage('Verify AWS Identity') {
            when {
                branch 'main'
            }

            steps {
                script {
                    env.AWS_ACCOUNT_ID = sh(
                        label: 'Resolve AWS account ID',
                        returnStdout: true,
                        script: '''#!/usr/bin/env bash
set -Eeuo pipefail

aws sts get-caller-identity \
  --query Account \
  --output text \
  --region "${AWS_REGION}"
'''
                    ).trim()

                    if (
                        env.AWS_ACCOUNT_ID !=
                        env.EXPECTED_AWS_ACCOUNT_ID
                    ) {
                        error(
                            "Wrong AWS account. Expected " +
                            "${env.EXPECTED_AWS_ACCOUNT_ID}, got " +
                            "${env.AWS_ACCOUNT_ID}"
                        )
                    }
                }

                sh(
                    label: 'Display AWS caller identity',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

aws --version

aws sts get-caller-identity \
  --region "${AWS_REGION}" \
  --output json |
  jq .
'''
                )
            }
        }

        stage('Resolve ECR Destination') {
            when {
                branch 'main'
            }

            steps {
                script {
                    env.ECR_REGISTRY =
                        "${env.AWS_ACCOUNT_ID}.dkr.ecr." +
                        "${env.AWS_REGION}.amazonaws.com"

                    env.ECR_COMMIT_IMAGE =
                        "${env.ECR_REGISTRY}/" +
                        "${env.ECR_REPOSITORY}:" +
                        "${env.IMAGE_TAG}"

                    env.ECR_STAGING_IMAGE =
                        "${env.ECR_REGISTRY}/" +
                        "${env.ECR_REPOSITORY}:staging"
                }

                echo "ECR registry: ${ECR_REGISTRY}"
                echo "Immutable image: ${ECR_COMMIT_IMAGE}"
                echo "Staging image: ${ECR_STAGING_IMAGE}"
            }
        }

        stage('Verify ECR Repository') {
            when {
                branch 'main'
            }

            steps {
                sh(
                    label: 'Verify ECR repository configuration',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

repository_json="$(
  aws ecr describe-repositories \
    --repository-names "${ECR_REPOSITORY}" \
    --region "${AWS_REGION}" \
    --output json
)"

echo "${repository_json}" |
  jq .

repository_uri="$(
  echo "${repository_json}" |
  jq -r '.repositories[0].repositoryUri'
)"

tag_mutability="$(
  echo "${repository_json}" |
  jq -r '.repositories[0].imageTagMutability'
)"

echo "Repository URI: ${repository_uri}"
echo "Tag mutability: ${tag_mutability}"

test \
  "${repository_uri}" = \
  "${ECR_REGISTRY}/${ECR_REPOSITORY}"

test \
  "${tag_mutability}" = \
  "IMMUTABLE_WITH_EXCLUSION"

echo "${repository_json}" |
  jq -e '
    any(
      .repositories[0]
      .imageTagMutabilityExclusionFilters[]?;
      .filterType == "WILDCARD" and
      .filter == "staging"
    )
  ' >/dev/null

echo
echo "ECR repository configuration verified."
'''
                )
            }
        }

        stage('Authenticate to ECR') {
            when {
                branch 'main'
            }

            steps {
                sh(
                    label: 'Authenticate Docker to Amazon ECR',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

aws ecr get-login-password \
  --region "${AWS_REGION}" |
docker login \
  --username AWS \
  --password-stdin "${ECR_REGISTRY}"
'''
                )
            }
        }

        stage('Publish Immutable Image') {
            when {
                branch 'main'
            }

            steps {
                sh(
                    label: 'Push immutable Git SHA image',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

existing_digest="$(
  aws ecr describe-images \
    --repository-name "${ECR_REPOSITORY}" \
    --image-ids "imageTag=${IMAGE_TAG}" \
    --region "${AWS_REGION}" \
    --query 'imageDetails[0].imageDigest' \
    --output text \
    2>/dev/null || true
)"

if [[ "${existing_digest}" == sha256:* ]]; then
  echo "Immutable image already exists:"
  echo "${ECR_COMMIT_IMAGE}"
  echo "Existing digest: ${existing_digest}"
  echo "The immutable tag will not be overwritten."
  exit 0
fi

docker tag \
  "${LOCAL_IMAGE_NAME}:${IMAGE_TAG}" \
  "${ECR_COMMIT_IMAGE}"

docker push \
  "${ECR_COMMIT_IMAGE}"

echo
echo "Immutable image pushed:"
echo "${ECR_COMMIT_IMAGE}"
'''
                )
            }
        }

        stage('Resolve Published Digest') {
            when {
                branch 'main'
            }

            steps {
                script {
                    env.ECR_IMAGE_DIGEST = sh(
                        label: 'Resolve ECR image digest',
                        returnStdout: true,
                        script: '''#!/usr/bin/env bash
set -Eeuo pipefail

image_digest=""

for attempt in $(seq 1 15); do
  echo \
    "Digest lookup attempt ${attempt}/15" \
    >&2

  image_digest="$(
    aws ecr describe-images \
      --repository-name "${ECR_REPOSITORY}" \
      --image-ids "imageTag=${IMAGE_TAG}" \
      --region "${AWS_REGION}" \
      --query 'imageDetails[0].imageDigest' \
      --output text \
      2>/dev/null || true
  )"

  if [[ "${image_digest}" == sha256:* ]]; then
    printf '%s' "${image_digest}"
    exit 0
  fi

  sleep 2
done

echo "Unable to resolve ECR image digest." >&2
exit 1
'''
                    ).trim()
                }

                echo "Published ECR digest: ${ECR_IMAGE_DIGEST}"
            }
        }

        stage('Verify Immutable Image') {
            when {
                branch 'main'
            }

            steps {
                sh(
                    label: 'Verify immutable image metadata',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

digest_image="$(
  printf '%s/%s@%s' \
    "${ECR_REGISTRY}" \
    "${ECR_REPOSITORY}" \
    "${ECR_IMAGE_DIGEST}"
)"

echo "Pulling immutable image:"
echo "${digest_image}"

docker pull \
  "${digest_image}"

remote_revision="$(
  docker image inspect \
    --format \
      '{{index .Config.Labels "org.opencontainers.image.revision"}}' \
    "${digest_image}"
)"

remote_source="$(
  docker image inspect \
    --format \
      '{{index .Config.Labels "org.opencontainers.image.source"}}' \
    "${digest_image}"
)"

echo "Remote revision: ${remote_revision}"
echo "Remote source: ${remote_source}"

test \
  "${remote_revision}" = \
  "${FULL_GIT_COMMIT}"

test \
  "${remote_source}" = \
  "${GIT_REMOTE_URL}"

echo
echo "Immutable image metadata verified."
'''
                )
            }
        }

        stage('Publish Staging Tag') {
            when {
                branch 'main'
            }

            steps {
                sh(
                    label: 'Update staging from immutable digest',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

immutable_digest_reference="$(
  printf '%s/%s@%s' \
    "${ECR_REGISTRY}" \
    "${ECR_REPOSITORY}" \
    "${ECR_IMAGE_DIGEST}"
)"

docker buildx imagetools create \
  --prefer-index=false \
  --tag "${ECR_STAGING_IMAGE}" \
  "${immutable_digest_reference}"

echo
echo "Staging tag updated:"
echo "${ECR_STAGING_IMAGE}"
'''
                )
            }
        }

        stage('Verify Staging Tag') {
            when {
                branch 'main'
            }

            steps {
                sh(
                    label: 'Verify staging and immutable digests',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

staging_digest=""

for attempt in $(seq 1 15); do
  echo "Staging lookup attempt ${attempt}/15"

  staging_digest="$(
    aws ecr describe-images \
      --repository-name "${ECR_REPOSITORY}" \
      --image-ids imageTag=staging \
      --region "${AWS_REGION}" \
      --query 'imageDetails[0].imageDigest' \
      --output text \
      2>/dev/null || true
  )"

  if [[ "${staging_digest}" == sha256:* ]]; then
    break
  fi

  sleep 2
done

echo "Immutable digest: ${ECR_IMAGE_DIGEST}"
echo "Staging digest: ${staging_digest}"

[[ "${staging_digest}" == sha256:* ]]

test \
  "${staging_digest}" = \
  "${ECR_IMAGE_DIGEST}"

echo
echo "Amazon ECR publication verification succeeded."
'''
                )
            }
        }
    }

    post {
        always {
            sh(
                label: 'Clean temporary Docker resources',
                script: '''#!/usr/bin/env bash
set +e

echo "Beginning Docker cleanup."

if [[ -n "${CONTAINER_NAME:-}" ]]; then
  if docker container inspect \
    "${CONTAINER_NAME}" \
    >/dev/null 2>&1; then

    echo
    echo "Final container logs:"

    docker logs \
      "${CONTAINER_NAME}" || true

    echo
    echo "Removing test container:"

    docker rm \
      --force \
      "${CONTAINER_NAME}" || true
  fi
fi

images_to_remove=()

if [[ -n "${IMAGE_TAG:-}" ]]; then
  images_to_remove+=(
    "${LOCAL_IMAGE_NAME}:${IMAGE_TAG}"
  )
fi

if [[ -n "${ECR_COMMIT_IMAGE:-}" ]]; then
  images_to_remove+=(
    "${ECR_COMMIT_IMAGE}"
  )
fi

if [[ -n "${ECR_STAGING_IMAGE:-}" ]]; then
  images_to_remove+=(
    "${ECR_STAGING_IMAGE}"
  )
fi

if [[ -n "${ECR_REGISTRY:-}" &&
      -n "${ECR_REPOSITORY:-}" &&
      -n "${ECR_IMAGE_DIGEST:-}" ]]; then

  images_to_remove+=(
    "${ECR_REGISTRY}/${ECR_REPOSITORY}@${ECR_IMAGE_DIGEST}"
  )
fi

for image in "${images_to_remove[@]}"; do
  echo
  echo "Removing local image reference:"
  echo "${image}"

  docker image rm \
    "${image}" || true
done

if [[ -n "${ECR_REGISTRY:-}" ]]; then
  echo
  echo "Logging out from Amazon ECR:"

  docker logout \
    "${ECR_REGISTRY}" || true
fi

echo
echo "Docker cleanup completed."
'''
            )

            deleteDir()
        }

        success {
            script {
                if (env.BRANCH_NAME == 'main') {
                    echo(
                        "Trusted main image published: " +
                        "${env.ECR_COMMIT_IMAGE}"
                    )

                    echo(
                        "Published digest: " +
                        "${env.ECR_IMAGE_DIGEST}"
                    )
                } else {
                    echo(
                        'Pull-request image passed local ' +
                        'Jenkins validation. No ECR push ' +
                        'was performed.'
                    )
                }
            }
        }

        failure {
            echo 'ASP.NET CI or Amazon ECR publication failed.'
        }

        aborted {
            echo 'ASP.NET CI or Amazon ECR publication was aborted.'
        }
    }
}