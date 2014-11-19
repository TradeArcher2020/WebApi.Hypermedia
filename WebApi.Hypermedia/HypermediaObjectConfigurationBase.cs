using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;
using System.Web.Http;

namespace WebApi.Hypermedia
{
    public interface IHypermediaObjectConfiguration
    {
        Type DtoType { get; }
        void Configure();

        IHypermediaObject Create(object dto);
    }

    public abstract class HypermediaObjectConfigurationBase<TDto> : IHypermediaObjectConfiguration
    {
        protected HypermediaObjectConfigurationBase()
        {
            this._metadata = new MetadataCollection<TDto>();
            this.Configure();
        }

        private MetadataCollection<TDto> _metadata;

        public MetadataCollection<TDto> Metadata
        {
            get { return _metadata; }
            set { _metadata = value; }
        }

        /// <summary>
        /// Implement this method to add default Links associated with the related TDto object.
        /// </summary>
        public abstract void Configure();

        /// <summary>
        /// This method creates a default instance of the HypermediaObject for a specific DTO type.
        /// It prepopulates the HypermediaObject's Links collection with Links defined in the configuration Metadata.  
        /// </summary>
        /// <param name="dto">The DTO for which this HypermediaObject is providing metadata.</param>
        /// <returns></returns>
        public IHypermediaObject Create(object dto)
        {
            //Create the HypermediaObject.
            var hypermediaObject = new HypermediaObject(dto);

            //Loop through the configured metadata
            foreach (var key in this.Metadata.Keys)
            {
                HashSet<Link> value;
                this.Metadata.TryGetValue(key, out value);

                if (value != null)
                {
                    //Add the links from the Metadata to the HypermediaObject's Links
                    foreach (var link in value)
                    {
                        hypermediaObject.Medadata.Add(link);
                    }
                }
            }

            return hypermediaObject;
        }

        public Type DtoType
        {
            get { return typeof(TDto); }
        }
    }

    public class MetadataCollection<TDto>
    {
        public HashSet<Link> Add(Expression<Func<TDto, object>> expression, HashSet<Link> links)
        {
            this._metadata.Add(ObjectExtensions.GetFullPropertyPathName(expression), links);
            return links;
        }
        public HashSet<Link> Add(Expression<Func<TDto, object>> expression, Link link)
        {
            HashSet<Link> links;
            if (this._metadata.ContainsKey(ObjectExtensions.GetFullPropertyPathName(expression)))
            {
                links = this._metadata[ObjectExtensions.GetFullPropertyPathName(expression)] ?? new HashSet<Link> { link };

                links.Add(link);
            }
            else
            {
                links = new HashSet<Link> { link };
                this._metadata.Add(ObjectExtensions.GetFullPropertyPathName(expression), links);
            }

            return links;
        }
        public HashSet<Link> Add(string fullPropertyPathName, HashSet<Link> links)
        {
            this._metadata.Add(fullPropertyPathName, links);
            return links;
        }
        public HashSet<Link> Add(string fullPropertyPathName, Link link)
        {
            HashSet<Link> links;
            if (this._metadata.ContainsKey(fullPropertyPathName))
            {
                links = this._metadata[fullPropertyPathName] ?? new HashSet<Link> { link };

                links.Add(link);
            }
            else
            {
                links = new HashSet<Link> { link };
                this._metadata.Add(fullPropertyPathName, links);
            }

            return links;
        }

        public bool Remove(Expression<Func<TDto, object>> expression, HashSet<Link> links)
        {
            return this._metadata.Remove(ObjectExtensions.GetFullPropertyPathName(expression));
        }

        public bool Remove(Expression<Func<TDto, object>> expression, Link link)
        {
            if (this._metadata.ContainsKey(ObjectExtensions.GetFullPropertyPathName(expression)))
            {
                this._metadata[ObjectExtensions.GetFullPropertyPathName(expression)].Remove(link);
            }
            return this._metadata.Remove(ObjectExtensions.GetFullPropertyPathName(expression));
        }

        public bool Remove(string fullPropertyPathName, HashSet<Link> links)
        {
            return this._metadata.Remove(fullPropertyPathName);
        }

        public bool Remove(string fullPropertyPathName, Link link)
        {
            if (this._metadata.ContainsKey(fullPropertyPathName))
            {
                this._metadata[fullPropertyPathName].Remove(link);
            }
            return this._metadata.Remove(fullPropertyPathName);
        }

        public void Clear()
        {
            this._metadata.Clear();
        }

        public void TryGetValue(Expression<Func<TDto, object>> expression, out HashSet<Link> links)
        {
            this._metadata.TryGetValue(ObjectExtensions.GetFullPropertyPathName(expression), out links);
        }

        public void TryGetValue(string fullPropertyPathName, out HashSet<Link> links)
        {
            this._metadata.TryGetValue(fullPropertyPathName, out links);
        }

        public bool ContainsKey(Expression<Func<TDto, object>> expression)
        {
            return this._metadata.ContainsKey(ObjectExtensions.GetFullPropertyPathName(expression));
        }

        public bool ContainsKey(string fullPropertyPathName)
        {
            return this._metadata.ContainsKey(fullPropertyPathName);
        }

        public Dictionary<string, HashSet<Link>>.KeyCollection Keys
        {
            get { return this._metadata.Keys; }
        }

        public Dictionary<string, HashSet<Link>>.ValueCollection Values
        {
            get { return this._metadata.Values; }
        }

        private readonly Dictionary<string, HashSet<Link>> _metadata = new Dictionary<string, HashSet<Link>>();
    }

    public static class HypermediaConfigurationManager
    {
        private static HttpConfiguration config;
        public static void Initialize(HttpConfiguration configuration)
        {
            config = configuration;
        }
        public static HypermediaObjectConfigurationBase<T> GetConfigurationFor<T>()
        {
            return GetConfigurationFor(typeof (T)) as HypermediaObjectConfigurationBase<T>;
        }
        public static IHypermediaObjectConfiguration GetConfigurationFor(Type type)
        {
            var hypermediaConfigurations = config.GetHypermediaObjectConfigurations();

            if (hypermediaConfigurations == null)
            {
                return null;
            }

            if (hypermediaConfigurations.ContainsKey(type))
            {
                return hypermediaConfigurations[type];
            }

            return null;
        }

        public static void SetConfigurationFor<T>(HypermediaObjectConfigurationBase<T> hypermediaConfiguration)
        {
            SetConfigurationFor((IHypermediaObjectConfiguration)hypermediaConfiguration);
        }

        public static void SetConfigurationFor(IHypermediaObjectConfiguration hypermediaConfiguration)
        {
            config.AddOrUpdateHypermediaObjectConfiguration(hypermediaConfiguration);
        }
    }
}