# What
Hosted services are background services that runs within the same process as an application using the .NET Generic Host.The .NET Generic Host is responsible for starting up an application and managing it's lifetime. 

A *host* is an object that encapsulate an application's resource and lifetime functionality and includes:
- Dependency Injection
- Logging
- Configuration
- Application shutdown
- IHostedService implementation

# Hosted Service Lifecycle
Hosted services has two main methods:
- StartAsync: This is called during application startup in registration order
- StopAsync: This is called during application shutdown in reverse registration order

# Background Service Lifecycle
Inherits from IHostedService:
- StartAsync: Implemented automatically, will call ExecuteAsync() as a background task
- ExecuteAsync: Where the user code will live
- StopAsync: Implemented automatically, signals cancellation to ExecuteAsync() and waits for completion

# Application Lifecycle

## 1. Registring services
Before starting a host, it needs to be made aware of any services so that it can set up its DI container.
```
var builder = Host.CreateApplicationBuilder();

builder.Services.AddHostedService<Worker>();
builderServices.AddHostedService<Worker2>();

var host = builder.Build();
```

## 2. Initialization 
When a host is ran, it will call each of the HostedService's `StartAsync` method. This is done in order of registration.

This is a blocking operation as the host waits for ALL `StartAsync()` methods to complete. If any of it fails, the entire application startup fails. It has a default timeout of 30 seconds.

```
host.Run();
```

## 3. Running
After all `StartAsync` methods complete, the application is marked as started. For `BackgroundService` implementations, the `ExecuteAsync()` methods will begin running concurrently in the background. The host will keep the application alive and monitor for shutdown signals.

Because `StartAsync` is blocking, `IHostedService` should be reserved for short, quick initialization tasks long-running work should be delegated to `BackgroundService` or if really needed, fire and forget.


## 4. Shudown
This is triggered by:
- A call to `IHostedApplicationLifetime.StopApplication()`
- User sends a SIGINT signal
- Unhandled exceptions

