# CyberArk Credential Provider - .NET + MySQL Integration

https://img.shields.io/badge/License-MIT-green.svg](https://choosealicense.com/licenses/mit/)
https://img.shields.io/badge/.NET-6.0-blue.svg](https://dotnet.microsoft.com/)
https://img.shields.io/badge/MySQL-Connector-orange.svg](https://www.nuget.org/packages/MySqlConnector/)
[![CyberArk](https://img.shields.io/badge/CordSDK-lightgrey.svg](https://docs.cyberark.com/credential-providers/latest/en/Content/CP%20and%20ASCP/Net-Application-Password-SDK.htm)

## 📘 Overview

This project demonstrates a practical example of a .NET application running on Ubuntu 20.04 that retrieves credentials securely using the CyberArk Credential Provider SDK and connects to a MySQL database to list all records from a table.

## 🚀 Features

- Secure credential retrieval via CyberArk Credential Provider
- Dynamic DLL loading and reflection
- MySQL database connection using retrieved credentials
- Full table data listing with dynamic column handling
- Null-safe and exception-resilient code

## 🛠️ Prerequisites

- Ubuntu 20.04 or compatible Linux distribution
- .NET SDK installed ([Install Guide](https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu))
tial Provider installed and configured
- Valid Application ID with access to a safe and object
- MySQL server accessible from the host

## 📦 Dependencies

Install required packages via NuGet:


dotnet add package netstandardpasswordsdk
dotnet add package MySqlConnector

