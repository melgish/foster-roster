﻿namespace FosterRoster.Shared.Models;

public interface IIdBearer
{
    /// <summary>
    /// Gets the ID of the entity.
    /// </summary>
    int Id { get; }
}

public sealed record IdOnlyDto(int Id) : IIdBearer;