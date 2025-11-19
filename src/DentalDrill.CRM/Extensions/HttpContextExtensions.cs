using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DentalDrill.CRM.Extensions
{
    public class HttpContextTemporaryDataMap
    {
        private readonly Dictionary<String, HttpContextTemporaryDataEntry> entriesMap = new Dictionary<String, HttpContextTemporaryDataEntry>();

        public void Store<T>(String key, T value)
        {
            var entry = new HttpContextTemporaryDataEntry(key, typeof(T), value);
            this.entriesMap.Add(key, entry);
        }

        public T Peek<T>(String key)
        {
            if (!this.entriesMap.TryGetValue(key, out var entry))
            {
                throw new InvalidOperationException($"Entry with key {key} not found");
            }

            if (entry.Type != typeof(T) && !typeof(T).IsAssignableFrom(entry.Type))
            {
                throw new InvalidOperationException($"Entry with key {key} has different type");
            }

            return (T)entry.Value;
        }

        public T Retrieve<T>(String key)
        {
            if (!this.entriesMap.TryGetValue(key, out var entry))
            {
                throw new InvalidOperationException($"Entry with key {key} not found");
            }

            if (entry.Type != typeof(T) && !typeof(T).IsAssignableFrom(entry.Type))
            {
                throw new InvalidOperationException($"Entry with key {key} has different type");
            }

            this.entriesMap.Remove(key);
            return (T)entry.Value;
        }
    }

    public class HttpContextTemporaryDataEntry
    {
        public HttpContextTemporaryDataEntry(String key, Type type, Object value)
        {
            this.Key = key;
            this.Type = type;
            this.Value = value;
        }

        public String Key { get; }

        public Type Type { get; }

        public Object Value { get; }
    }

    public static class HttpContextExtensions
    {
        public static Boolean IsAjaxRequest(this HttpContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException($"{nameof(context)} is null", nameof(context));
            }

            return context.Request.IsAjaxRequest();
        }

        public static Boolean IsAjaxRequest(this HttpRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException($"{nameof(request)} is null", nameof(request));
            }

            if (request.Headers != null && request.Headers.TryGetValue("X-Requested-With", out var requestedWith))
            {
                return requestedWith == "XMLHttpRequest";
            }

            return false;
        }

        public static void StoreTemporaryData<T>(this HttpContext context, String key, T value)
        {
            var map = context.Items["__HttpContextTemporaryDataMap"] as HttpContextTemporaryDataMap;
            if (map == null)
            {
                map = new HttpContextTemporaryDataMap();
                context.Items["__HttpContextTemporaryDataMap"] = map;
            }

            map.Store(key, value);
        }

        public static T PeekTemporaryData<T>(this HttpContext context, String key)
        {
            var map = context.Items["__HttpContextTemporaryDataMap"] as HttpContextTemporaryDataMap;
            if (map == null)
            {
                map = new HttpContextTemporaryDataMap();
                context.Items["__HttpContextTemporaryDataMap"] = map;
            }

            return map.Peek<T>(key);
        }

        public static T RetrieveTemporaryData<T>(this HttpContext context, String key)
        {
            var map = context.Items["__HttpContextTemporaryDataMap"] as HttpContextTemporaryDataMap;
            if (map == null)
            {
                map = new HttpContextTemporaryDataMap();
                context.Items["__HttpContextTemporaryDataMap"] = map;
            }

            return map.Retrieve<T>(key);
        }
    }
}
