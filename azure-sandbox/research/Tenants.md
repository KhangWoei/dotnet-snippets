# Tenants

A tenant is a dedicated, isolated instance of Microsoft Entra ID representing a single organization. It is created automatically when an organization signs up for a Microsoft cloud service such as Azure, Microsoft 365, or Dynamics 365.

Every tenant has a unique **Tenant ID** (GUID) and a default domain name in the form `contoso.onmicrosoft.com`. All users, groups, devices, app registrations, and policies in Entra ID belong to a tenant.

A tenant can be trusted by multiple [Subscriptions](./Subscriptions), but each subscription is associated with exactly one tenant. The subscription delegates authentication and authorization to its trusted tenant — only identities that exist in or are guests of that tenant can be granted access to the subscription's resources.

**Multi-tenant** applications can accept sign-ins from users in any tenant, rather than being limited to a single organization's directory.
