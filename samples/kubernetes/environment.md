# Establishing a Kubernetes environment

The [samples](./README.md) rely on a functioning [Kubernetes](https://kubernetes.io/) environment and [kubectl](https://kubernetes.io/docs/reference/kubectl/) to manage it.

## Local environment

If you are new to Kubernetes, you'll likely want to start with a [local environment](https://kubernetes.io/docs/tasks/tools/), such as [Docker Desktop](https://docs.docker.com/desktop/kubernetes/), [K3s](https://k3s.io/), [Minikube](https://minikube.sigs.k8s.io/docs/), [OpenShift Local](https://developers.redhat.com/products/openshift-local), or [Rancher](https://rancherdesktop.io/).

Isolated Kubernetes environments make it easy to install one or more apps, and delete all the state with a single gesture afterwords. For example, minikube makes that easy and [offers a copy of `kubectl`](https://minikube.sigs.k8s.io/docs/handbook/kubectl/) so that you don't need to install it separately (however, it still uses the global kubectl configuration).

## Switch between environments

`kubectl` can be used to switch between contexts to test differences in behavior between clusters. The most natural way of doing that is to maintain multiple `kubectl` contexts and switch between them.

```bash
$ kubectl config get-contexts
CURRENT   NAME             CLUSTER          AUTHINFO         NAMESPACE
*         docker-desktop   docker-desktop   docker-desktop   
          minikube         minikube         minikube         default
$ kubectl config use-context minikube
Switched to context "minikube".
$ kubectl config get-contexts        
CURRENT   NAME             CLUSTER          AUTHINFO         NAMESPACE
          docker-desktop   docker-desktop   docker-desktop   
*         minikube         minikube         minikube         default
```

This example shows switching between local clusters. The same technique can be used with cloud environments.

## Register a cloud environment

`kubectl` can manage a Kubernetes cluster for a cloud service.

For Azure Kubernetes Service (AKS), you can do that via the [Azure CLI](https://learn.microsoft.com/azure/aks/learn/quick-kubernetes-deploy-cli#connect-to-the-cluster). This same command is available via the "Connect" menu in the Azure Portal (for an AKS resource).

Other cloud services have similar experiences.
