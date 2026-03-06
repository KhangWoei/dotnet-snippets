# Resource Groups

A resource group is a logical container that holds related Azure resources within a [Subscription](./Subscriptions). Every Azure resource must belong to exactly one resource group.

*** Deleting a resource group deletes all resources inside it ***

The resource group itself has a **region**, which determines where its metadata is stored, but resources within it can be in any Azure region. RBAC roles assigned at the resource group level are inherited by all resources within it.

The primary design principle is to group resources by lifecycle, resources that are deployed, updated, and deleted together belong in the same group. Common organization strategies include grouping by application, by environment (dev/prod), or by team. Supports **tags** (key-value pairs) for cost tracking, ownership labeling, and automation.
