using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Crosscutting.Caching
{
    public interface ICaching
    {
        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <returns>The value associated with the specified key.</returns>
        Task<T> GetAsync<T>(string key);

        /// <summary>
        /// Gets the value associated with the specified keys.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="keys">The key of the value to get.</param>
        /// <returns>The values associated with the specified keys.</returns>
        Task<IDictionary<string, T>> GetAsync<T>(string[] keys);

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        /// <param name="cacheTimeInSecond">Cache time</param>
        /// <param name="isAbsoluteExpiration">Is Absolute Expiration</param>
        Task SetAsync<T>(string key, T data, double expirationInMinutes);

        /// <summary>
        /// Adds the specified key and object to the cache.
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="data">Data</param>
        Task SetAsync<T>(string key, T data);

        /// <summary>
        /// Adds multiple key and object to the cache.
        /// </summary>
        /// <param name="keyValues"></param>
        Task SetAsync<T>(IDictionary<string, T> keyValues);

        /// <summary>
        /// Adds multiple key and object to the cache.
        /// </summary>
        /// <param name="keyValues"></param>
        Task SetAsync<T>(IDictionary<string, T> keyValues, double expirationInMinutes);

        /// <summary>
        /// Gets a value indicating whether the value associated with the specified key is cached
        /// </summary>
        /// <param name="key">key</param>
        /// <returns>Result</returns>
        Task<bool> ExistsAsync(string key);

        /// <summary>
        /// Removes the value with the specified key from the cache
        /// </summary>
        /// <param name="key">/key</param>
        Task RemoveAsync(string key);


        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// If the element doesn't exist, create it.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="factory">Create</param>
        /// <returns>The value associated with the specified key.</returns>
        Task<T> GetOrCreate<T>(string key, Func<T> factory);

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// If the element doesn't exist, create it.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="factory">Create</param>
        /// <param name="cacheTimeInSecond">Cache time</param>
        /// <param name="isAbsoluteExpiration">Is Absolute Expiration</param>
        /// <returns>The value associated with the specified key.</returns>
        Task<T> GetOrCreate<T>(string key, Func<T> factory, int cacheTimeInSecond, bool isAbsoluteExpiration = true);

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// If the element doesn't exist, create it Async.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="factory">Create</param>
        /// <returns>The value associated with the specified key.</returns>
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory);

        /// <summary>
        /// Gets or sets the value associated with the specified key.
        /// If the element doesn't exist, create it Async.
        /// </summary>
        /// <typeparam name="T">Type</typeparam>
        /// <param name="key">The key of the value to get.</param>
        /// <param name="factory">Create</param>
        /// <param name="cacheTimeInSecond">Cache time</param>
        /// <param name="isAbsoluteExpiration">Is Absolute Expiration</param>
        /// <returns>The value associated with the specified key.</returns>
        Task<T> GetOrCreateAsync<T>(string key, Func<Task<T>> factory, int cacheTimeInSecond, bool isAbsoluteExpiration = true);
    }
}
