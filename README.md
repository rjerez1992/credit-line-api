# Credit Line API
Basic API for credit line validation for customers based on given rules.

## Overview

The purpose is to determine the credit line of customers given some specific values, related to the type of customer and other finnancial data.

A "identifier" is requested on each application, this helps to manage the rules related to the same application beign sent more than once (ex: Once an application is accepted, the return values must be the same regarding any parameter changes). It also helps with the rules for the rate limiting, as it is only applied once an application has been procesed. No rate limit rules are defined for newly received applications.
A basic endpoint that fetches and GUID was created for this purpose, but it could be possible for any external system to create those ids.

The project was separated into two sections, one for the service itself and one for the test suit (using xUnit). A string resource file was used to manage text messages that would be returned by the API.

On the service project it is a basic MVC application.For managing the logic of the credit line application, a service was created, which is managed by the controller that serves the requests. 
For the rate limit, a basic middleware was created. It reads the application from the request and applies the limiting rules defined.

The test suit is oriented towards verifying the rules for the credit-line acceptance.

*A Postman collection is included in the repository for easy access to testing the endpoints available*

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

