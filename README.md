![.NET 5](https://github.com/aimenux/WorkerServiceLoggingDemo/workflows/.NET%205/badge.svg)

# WorkerServiceLoggingDemo
```
Enabling logging based on microsoft built-in tooling for long-running cross-platform services
```

> In this repo, i m enabling logging for long-running background services (based on [worker service template](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/host/hosted-services?view=aspnetcore-5.0&tabs=visual-studio#worker-service-template)).
>
> Two ways are provided in order to target console and application-insights based on microsoft built-in tooling :
>
>> :pushpin: `BasicWay` : use exclusively `Microsoft.Extensions.Logging` and `ILogger`
>>
>> :pushpin: `AdvanceWay` : use also `Microsoft.ApplicationInsights.WorkerService` and `TelemetryClient`
>
> Logging targets used in the demo are : 
>
>> :pushpin: console target
>>
>> :pushpin: application insights target
>

**`Tools`** : vs19, net 5.0