# Http Client
As of the 14th of March 2023, Microsoft has recommended the use of IHttpClientFactory. See [here](https://learn.microsoft.com/en-us/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests) for more details.

Temporary solution is to use a static or singleton with a speicfied PooledConnectoinLifetime.
