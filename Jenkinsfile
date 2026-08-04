pipeline {
    agent {
        label 'dotnet-docker-agent'
    }

    stages {
        stage('Agent Verification') {
            steps {
                sh(
                    label: 'Verify Jenkins agent',
                    script: '''#!/usr/bin/env bash
set -Eeuo pipefail

echo "Jenkins node: ${NODE_NAME}"
echo "Linux user: $(whoami)"
echo "Hostname: $(hostname)"

id
docker version
'''
                )
            }
        }
    }
}