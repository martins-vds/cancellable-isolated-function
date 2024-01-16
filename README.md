# Proof of Concept: Azure Function with Cancellation

This repository contains a proof of concept for an Azure Function that can be cancelled after a specified number of seconds.

## Prerequisites

Before running the Azure Function, make sure you have the following prerequisites installed:

- [Azure Functions Core Tools](https://docs.microsoft.com/en-us/azure/azure-functions/functions-run-local?tabs=windows%2Ccsharp%2Cbash)

## Getting Started

1. Clone this repository to your local machine:

    ```bash
    git clone https://github.com/martins-vds/poc-az-function-isolated-worker.git
    ```

2. Navigate to the project directory:

    ```bash
    cd poc-az-function-isolated-worker
    cd cancellable-isolated-function
    ```

3. Run the Azure Function locally:

    ```bash
    func start
    ```

## Usage

To trigger the cancellation of the Azure Function after a specific number of seconds, send an HTTP GET request to the following endpoint:

```bash
http://localhost:7071/api/cancel-me?wait=10
```

The `wait` query parameter specifies the number of seconds (between 1 and 30) to wait before completing the request. If the `wait` query parameter is not specified, the default value of 10 seconds is used.

When the Azure Function is cancelled before it completes, the following message is returned in the console:

```bash
Operation was cancelled. Reason: A task was canceled.
```

However, if the Azure Function is not cancelled before it completes, the following message is returned:

```bash
Welcome to Azure Functions!
```
