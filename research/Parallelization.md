# Parallelization

## Option 1 - Fire and Forget
Create an orchestration service that can dispatch a WebCrawl task for a unique base uri. It'll have to be responsible for:
    - Keeping track of all base URIs
    - Asynchronously dispatching WebCrawl tasks (and forget about it, doesn't really care about the result)

### Benefits
    - Really easy to implement

### Problems:
    - Can't control concurrency
        - No queuing
        - Don't know how many crawls are in process
    - Probably can't cancel or monitor it
    - Error handling

## Option 2 - Background queue service
Long-running service, where work items can be queued and worked on sequentially as previous work items are completed. Relying on the Worker Service template, see [here](https://learn.microsoft.com/en-us/dotnet/core/extensions/queue-service)

### Benefits
    - Basically negates the problems of Fire and Forget
    
