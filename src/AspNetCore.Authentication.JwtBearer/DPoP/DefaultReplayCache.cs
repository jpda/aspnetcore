// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using Microsoft.Extensions.Caching.Distributed;

namespace Duende.AspNetCore.Authentication.JwtBearer.DPoP;

/// <summary>
/// Default implementation of the replay cache using IDistributedCache
/// </summary>
public class DefaultReplayCache : IReplayCache
{
    private const string Prefix = "DPoPJwtBearerEvents-DPoPReplay-jti-";

    private readonly IDistributedCache _cache;

    /// <summary>
    /// Constructs new instances of <see cref="DefaultReplayCache"/>.
    /// </summary>
    public DefaultReplayCache(IDistributedCache cache)
    {
        _cache = cache;
    }

    /// <inheritdoc />
    public async Task Add(string handle, DateTimeOffset expiration, CancellationToken cancellationToken)
    {
        var options = new DistributedCacheEntryOptions
        {
            AbsoluteExpiration = expiration
        };

        await _cache.SetAsync(Prefix + handle, [], options, cancellationToken);
    }

    /// <inheritdoc />
    public async Task<bool> Exists(string handle, CancellationToken cancellationToken)
    {
        return await _cache.GetAsync(Prefix + handle, cancellationToken) != null;
    }
}