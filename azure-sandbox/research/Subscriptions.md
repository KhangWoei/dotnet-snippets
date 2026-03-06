# Subscriptions

An Azure subscription is a billing and management boundary for Azure resources. Every Azure resource, virtual machines, databases, storage accounts, etc. is deployed within a subscription.

Each subscription has a unique **Subscription ID** (GUID) and is associated with exactly one [Tenant](./Tenants). The tenant acts as the identity authority for the subscription; only users, groups, and service principals from that tenant (or its guests) can be granted access to the subscription's resources. A single tenant can, however, be trusted by many subscriptions.

Subscriptions also provide an isolation boundary for access control. RBAC roles can be scoped to the subscription level, and permissions granted at that scope are inherited by all resource groups and resources within it.

Common patterns include separate subscriptions per environment (dev, staging, prod) or per business unit, which provides both billing separation and stronger access isolation. Resources within a subscription are organized into [Resource Groups](./Resource%20Groups).
