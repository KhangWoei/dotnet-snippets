# Managed Identity

A managed identity is a special type of [Service Principal](./Service%20Princiapls) that is automatically created and managed by Azure for an Azure resource. It eliminates the need to store, rotate, or manage credentials in code or configuration — Azure handles the complete credential lifecycle transparently.

*** Recommended method for programmatic access from Azure resources ***

## Types

**System-assigned**: enabled directly on a single Azure resource. Its lifecycle is tied to that resource; when the resource is deleted, the managed identity is deleted automatically.

**User-assigned**: created as a standalone Azure resource and assigned to one or more Azure resources. Its lifecycle is independent of any single resource, making it suitable for scenarios where multiple resources share one identity or where resources are frequently recreated.

## How It Works

Application code obtains an access token by calling the **Instance Metadata Service (IMDS)** endpoint (`http://169.254.169.254/metadata/identity/oauth2/token`) from within the Azure compute host. Azure validates the request based on the resource's known identity and returns a bearer token. The caller never handles any credential — only the token.

In practice, Azure SDK clients (e.g., `ManagedIdentityCredential` in `Azure.Identity`) handle the IMDS call, token caching, and refresh automatically.

Supported Azure resources include VMs, App Service, Azure Functions, AKS, Container Apps, Logic Apps, and Data Factory, among others.
