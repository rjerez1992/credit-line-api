# Credit Line API
Basic API for credit line validation for customers based on given rules.

## Requirements

[.NET 6.0](https://dotnet.microsoft.com/en-us/download/dotnet/6.0) is required.
The project was tested on Visual Studio 2022 for Windows.

## How to run

Navigate to the project folder, then build the project

```console
$ dotnet build
```

To run the service use the following command. By default it runs on port 5000, but it can be changed from within the configuration file.

```console
$ dotnet run --project CreditLineAPI
```

To run the tests use the following command

```console
$ dotnet test CreditLineAPI.Tests
```

## Example requests

Get UUID
```console
curl --location --request GET 'localhost:5000/credit-line/id'
```

Send application
```console
curl --location --request POST 'localhost:5000/credit-line/apply' \
--header 'Content-Type: application/json' \
--data-raw '{
    "id": "d378cf76-dbdb-4289-9de8-66577bed70db",
    "foundingType": "SME",
    "cashBalance": 435.30,
    "monthlyRevenue": 4235.45,
    "requestedCreditLine": 100,
    "requestedDate": "2021-07-19T16:32:59.860Z"
}'
```

