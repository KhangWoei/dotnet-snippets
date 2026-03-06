# What is Entra

Microsoft Entra ID (formerly Azure Active Directory) is Microsoft's cloud-based identity and access management service. It manages users, groups, and application access across Microsoft cloud services including Azure, Microsoft 365, and Dynamics 365. Every Entra ID instance is scoped to a [Tenant](./Tenants) — a dedicated, isolated directory representing a single organization.

## Authentication Methods

### Password
*** This is not recommended *** 

Authentication to Azure data source with Microsoft Entra ID or federated Microsoft Entra users. Uses User Id amd Password.


### Integrated 
Requires an on-premise active direcotry instance that is joined to Microsoft Entra ID in the cloud. 

This uses the windows user you are currently logged in as.

### Interactive (MFA)
Will redirect users to an Azure authentication screen and ask them to enter valid credentials.

### Service Principal
Uses the credentials for a service principal, a service principal can be an application or a user. Read more on [Service Principals](./Service%20Princiapls)

### Managed Identity
*** Recomended method for programmmatic access to SQL ***
Client applications use the system or user assigned managed identity of a resource. This is essentially assigning a service principal to an azure resource. 

A service principal can be assigned to many resources. 

Read more on [Managed Identity](./Managed%20Identity)

### Device code
Enables client applications that does not have access to an interactive browser to authenticate by performing the authentication on a different device. 


### Token
We have authenticated ourselves externally, generated a JWT token and passed it to the application, that way the application does not have to do any authentication on it's part. 

