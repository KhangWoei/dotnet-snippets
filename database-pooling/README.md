# Motivation
I have a scenario where my tests needs an empty copy of a database. However, creating a new database for each test instance can be time and space consuming. This is an approach to pooling databases and reusing them. 

The approach is simple, have a DatabasePool type that needs to be consumed as a singleton. It will have a dictionary of databases it is monitoring and has a background thread that polls the for database availability frequently and updates the main thread whenever one is available.

The main thread, will check the current cache for what is available, if there is nothing available, it will try and create one provided it doesnt exceed that maximum poolsize. If it cannot create one, it will simply wait until one is available.

Maximum poolsize can be set either constrained by the number of databases or the current available space on disk.

## Error states:
### 1. What happens if we're polling but there are no databases under watch?

From the poller's perspective it doesn't care, but the main thread should check that if it tries to create a database, and fails, and if there are no databases under watch, it is an error state as it means we probably aren't able to provision a database.
