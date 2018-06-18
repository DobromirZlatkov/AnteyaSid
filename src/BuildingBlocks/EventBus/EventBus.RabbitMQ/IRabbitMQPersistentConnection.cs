﻿using RabbitMQ.Client;
using System;

namespace AnteyaSidOnContainers.BuildingBlocks.EventBus.EventBus.RabbitMQ
{
    public interface IRabbitMQPersistentConnection
       : IDisposable
    {
        bool IsConnected { get; }

        bool TryConnect();

        IModel CreateModel();
    }
}
