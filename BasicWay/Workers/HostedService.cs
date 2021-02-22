﻿using System;
using System.Threading;
using System.Threading.Tasks;
using BasicWay.Services;
using Microsoft.Extensions.Hosting;

namespace BasicWay.Workers
{
    public class HostedService : BackgroundService
    {
        private readonly IDummyService _service;

        private static readonly TimeSpan Delay = TimeSpan.FromSeconds(5);

        public HostedService(IDummyService service)
        {
            _service = service;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await _service.DoNothingAsync();
                await Task.Delay(Delay, stoppingToken);
            }
        }
    }
}
