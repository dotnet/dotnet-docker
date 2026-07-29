pipeline {
    agent {
        label 'linux && x86_64 && build-agent && docker'
    }

    environment {
        IMAGE_NAME = 'aspnetapp-ci'
        HOST_PORT = '18080'
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
                checkout scm
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

                echo "Image: ${IMAGE_NAME}:${IMAGE_TAG}"
                echo "Container: ${CONTAINER_NAME}"
            }
        }

        stage('Verify Docker') {
            steps {
                sh(
                    label: 'Verify Docker environment',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

echo "Build user:"
whoami

echo
echo "Build user groups:"
id

echo
echo "Docker version:"
docker version

echo
echo "Docker builder:"
docker buildx version

echo
echo "Docker daemon:"
docker info --format 'Server={{.ServerVersion}} Driver={{.Driver}} Logging={{.LoggingDriver}}'
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

echo "ASP.NET application files:"
ls -la samples/aspnetapp

echo
echo "Dockerfile:"
sed -n '1,200p' samples/aspnetapp/Dockerfile
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
  --label "org.opencontainers.image.revision=${GIT_COMMIT}" \
  --label "org.opencontainers.image.source=${GIT_URL:-unknown}" \
  --label "org.opencontainers.image.created=$(date -u +%Y-%m-%dT%H:%M:%SZ)" \
  --tag "${IMAGE_NAME}:${IMAGE_TAG}" \
  samples/aspnetapp
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

docker image inspect "${IMAGE_NAME}:${IMAGE_TAG}"

image_user="$(
  docker image inspect \
    --format '{{.Config.User}}' \
    "${IMAGE_NAME}:${IMAGE_TAG}"
)"

echo "Configured image user: ${image_user}"

test -n "${image_user}"
test "${image_user}" != "0"
test "${image_user}" != "root"

echo
echo "Image size:"
docker image inspect \
  --format '{{.Size}} bytes' \
  "${IMAGE_NAME}:${IMAGE_TAG}"

echo
echo "Image successfully uses a non-root user."
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

docker rm -f "${CONTAINER_NAME}" >/dev/null 2>&1 || true

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

docker ps \
  --filter "name=${CONTAINER_NAME}"
'''
                )
            }
        }

        stage('Container Smoke Test') {
            steps {
                sh(
                    label: 'Verify ASP.NET HTTP response',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

response_file="${WORKSPACE}/environment-response.json"
application_url="http://127.0.0.1:${HOST_PORT}/Environment"

application_ready=false

for attempt in $(seq 1 30); do
  echo "Smoke-test attempt ${attempt}/30"

  if curl \
    --fail \
    --silent \
    --show-error \
    "${application_url}" \
    --output "${response_file}"; then

    application_ready=true
    break
  fi

  sleep 2
done

if [[ "${application_ready}" != "true" ]]; then
  echo "Application did not become ready."

  echo
  echo "Container status:"
  docker ps -a \
    --filter "name=${CONTAINER_NAME}"

  echo
  echo "Container logs:"
  docker logs "${CONTAINER_NAME}" || true

  exit 1
fi

echo
echo "Application response:"
jq . "${response_file}"

jq -e '
  .runtimeVersion != null and
  .osArchitecture != null and
  .user != null
' "${response_file}" >/dev/null

echo
echo "ASP.NET smoke test completed successfully."
'''
                )
            }
        }

        stage('Runtime Security Check') {
            steps {
                sh(
                    label: 'Verify container security settings',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

response_file="${WORKSPACE}/environment-response.json"

runtime_user="$(
  jq -r '.user' "${response_file}"
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

echo "Runtime application user: ${runtime_user}"
echo "Security options: ${no_new_privileges}"
echo "Dropped capabilities: ${dropped_capabilities}"
echo "Port bindings: ${port_binding}"

test "${runtime_user}" != "root"

echo "${no_new_privileges}" |
  grep -q 'no-new-privileges'

echo "${dropped_capabilities}" |
  grep -q 'ALL'

echo
echo "Runtime security checks completed successfully."
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

if [[ -n "${CONTAINER_NAME:-}" ]]; then
  echo "Container logs:"
  docker logs "${CONTAINER_NAME}" || true

  echo
  echo "Removing test container:"
  docker rm -f "${CONTAINER_NAME}" || true
fi

if [[ -n "${IMAGE_TAG:-}" ]]; then
  echo
  echo "Removing temporary image:"
  docker image rm "${IMAGE_NAME}:${IMAGE_TAG}" || true
fi

docker image prune --force || true
'''
            )

            deleteDir()
        }

        success {
            echo 'ASP.NET container image passed all local Jenkins validation.'
        }

        failure {
            echo 'ASP.NET container image validation failed.'
        }
    }
}