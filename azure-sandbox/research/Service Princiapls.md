# Service Principals

A service principal is a security identity used by an application, service, or automated tool to access Azure resources within a specific tenant. Unlike a user account, a service principal is designed for non-interactive (programmatic) use.

## Types

**Application**: created when an app is registered in Entra ID; represents the app identity within a tenant. Authenticates via client secret or certificate.

**Managed Identity**: automatically created and managed by Azure for an Azure resource; no credentials to store or rotate. See [Managed Identity](./Managed%20Identity).

**Legacy**: older service principals without a corresponding app registration. Rarely encountered and deprecated for new workloads.

## Authentication

Application service principals authenticate using either a **client secret** or a **certificate**. Certificates are preferred, they cannot be accidentally embedded in code or config files.

## Access

A service principal can be assigned RBAC roles on Azure resources, resource groups, or subscriptions. Assignments are scoped using the service principal's **Object ID**. Each service principal has a **Client ID** (Application ID) and lives within a single tenant.
