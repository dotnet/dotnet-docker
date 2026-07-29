pipeline {
    agent {
        label 'linux && x86_64 && build-agent'
    }

    options {
        skipDefaultCheckout(true)
        timestamps()
        disableConcurrentBuilds()
        timeout(time: 10, unit: 'MINUTES')
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

        stage('Show SCM Context') {
            steps {
                sh(
                    label: 'Display source control context',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

echo "Job name: ${JOB_NAME}"
echo "Build number: ${BUILD_NUMBER}"
echo "Jenkins node: ${NODE_NAME}"
echo "Branch name: ${BRANCH_NAME}"
echo "Git commit: ${GIT_COMMIT:-not-set}"
echo "Pull request ID: ${CHANGE_ID:-not-a-pull-request}"
echo "Pull request target: ${CHANGE_TARGET:-not-a-pull-request}"

echo
echo "Current Git commit:"
git log -1 --oneline
'''
                )
            }
        }

        stage('Validate Repository') {
            steps {
                sh(
                    label: 'Validate ASP.NET repository structure',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

test -f Jenkinsfile
test -d samples/aspnetapp
test -f samples/aspnetapp/Dockerfile

echo "ASP.NET project directory found:"
ls -la samples/aspnetapp

echo
echo "Repository validation completed successfully."
'''
                )
            }
        }
    }

    post {
        success {
            echo 'GitHub and Jenkins Multibranch integration is working.'
        }

        failure {
            echo 'Multibranch bootstrap validation failed.'
        }

        always {
            deleteDir()
        }
    }
}