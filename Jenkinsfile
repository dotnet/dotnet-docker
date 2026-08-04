pipeline {
    agent {
        label 'dotnet-docker-agent'
    }

    environment {
        IMAGE_NAME = 'aspnetapp-ci'
        HOST_PORT  = '18080'
    }

    options {
        skipDefaultCheckout(true)
        timestamps()
        disableConcurrentBuilds()
        timeout(time: 20, unit: 'MINUTES')

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
                    /*
                     * checkout scm returns Git information as a map.
                     * We store the values explicitly because GIT_COMMIT and
                     * GIT_URL are not always available in later stages.
                     */
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
                    env.IMAGE_TAG = sh(
                        label: 'Calculate immutable image tag',
                        returnStdout: true,
                        script: '''#!/usr/bin/env bash
set -Eeuo pipefail

git rev-parse --short=12 HEAD
'''
                    ).trim()

                    env.CONTAINER_NAME =
                        "aspnetapp-${env.BUILD_NUMBER}-${env.IMAGE_TAG}"
                }

                echo "Jenkins node: ${NODE_NAME}"
                echo "Git commit: ${FULL_GIT_COMMIT}"
                echo "Git repository: ${GIT_REMOTE_URL}"
                echo "Docker image: ${IMAGE_NAME}:${IMAGE_TAG}"
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
echo "Docker daemon information:"
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
echo "Dockerfile contents:"
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
  --tag "${IMAGE_NAME}:${IMAGE_TAG}" \
  samples/aspnetapp

echo
echo "Created image:"

docker image ls \
  --filter "reference=${IMAGE_NAME}:${IMAGE_TAG}"
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

full_image_name="${IMAGE_NAME}:${IMAGE_TAG}"

echo "Inspecting image:"
echo "${full_image_name}"

echo
echo "Image ID:"
docker image inspect \
  --format '{{.Id}}' \
  "${full_image_name}"

echo
echo "Image creation time:"
docker image inspect \
  --format '{{.Created}}' \
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

# Remove a leftover container with the same name, if one exists.
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
  "${IMAGE_NAME}:${IMAGE_TAG}"

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

    echo
    echo "Container status:"

    docker ps \
      --all \
      --filter "name=^/${CONTAINER_NAME}$"

    echo
    echo "Container logs:"

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

  echo
  echo "Container status:"

  docker ps \
    --all \
    --filter "name=^/${CONTAINER_NAME}$"

  echo
  echo "Container logs:"

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
echo "Runtime application user: ${runtime_user}"
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

    docker logs "${CONTAINER_NAME}" || true

    echo
    echo "Removing test container:"

    docker rm \
      --force \
      "${CONTAINER_NAME}" || true
  else
    echo "Test container does not exist."
  fi
fi

if [[ -n "${IMAGE_TAG:-}" ]]; then
  if docker image inspect \
    "${IMAGE_NAME}:${IMAGE_TAG}" \
    >/dev/null 2>&1; then

    echo
    echo "Removing temporary image:"

    docker image rm \
      "${IMAGE_NAME}:${IMAGE_TAG}" || true
  else
    echo "Temporary image does not exist."
  fi
fi

echo
echo "Docker cleanup completed."
'''
            )

            deleteDir()
        }

        success {
            echo 'ASP.NET container image passed all local Jenkins validation.'
        }

        unstable {
            echo 'ASP.NET container validation completed with unstable results.'
        }

        failure {
            echo 'ASP.NET container image validation failed.'
        }

        aborted {
            echo 'ASP.NET container image validation was aborted.'
        }
    }
}