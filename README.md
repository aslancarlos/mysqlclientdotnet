
# CyberArk Credential Provider - .NET Example on Ubuntu 20.04

This repository contains a quick and practical example of a .NET application for recovering secrets via CyberArk Credential Provider (agent) and querying a MySQL database.

Pre:

  You must have the Credential Provider installed, with an Application ID with permissions in a safe environment.


Remembering that the account must be real and accessible by the host it will run.


# CyberArk Credential Provider - .NET Example

This repository contains a quick and practical example of a .NET application for recovering secrets via CyberArk Credential Provider (agent) and querying a MySQL database.

Pre:

  You must have the Credential Provider installed, with an Application ID with permissions in a safe environment.


Remembering that the account must be real and accessible by the host it will run.


## Badges

Add badges from somewhere like: [shields.io](https://shields.io/)

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](https://choosealicense.com/licenses/mit/)
[![GPLv3 License](https://img.shields.io/badge/License-GPL%20v3-yellow.svg)](https://opensource.org/licenses/)
[![AGPL License](https://img.shields.io/badge/license-AGPL-blue.svg)](http://www.gnu.org/licenses/agpl-3.0)


## Deployment

To deploy this project run

```bash
  dotnet add package netstandardpasswordsdk
  dotnet add package MySqlConnector 
  dotnet run
```


## Acknowledgements

 - [CyberArk Central Credential Provider - SDK .Net](https://docs.cyberark.com/credential-providers/latest/en/Content/CP%20and%20ASCP/Net-Application-Password-SDK.htm?tocpath=Developer%7CCredential%20Provider%7CApplication%20Password%20SDKs%7C.NET%20Application%20Password%20SDK%7C_____0)
 - [Install the .NET SDK or the .NET Runtime on Ubuntu](https://learn.microsoft.com/en-us/dotnet/core/install/linux-ubuntu)


