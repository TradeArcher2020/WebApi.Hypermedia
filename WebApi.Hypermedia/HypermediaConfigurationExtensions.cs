using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http;

namespace WebApi.Hypermedia
{
    public static class HypermediaConfigurationExtensions
    {
        public const string HypermediaDefaultConfigurations = "HypermediaDefaultConfigurations";
        public const string HypermediaContextualLinks = "HypermediaContextualLinks";
        public const string OverridingHypermediaObjectForContext = "OverridingHypermediaObjectForContext";
        public const string MergeHypermediaObjectConfiguration = "MergeHypermediaObjectConfiguration";

        public static bool IsAcceptSupported(this HttpRequestMessage request)
        {
            var config = request.GetConfiguration();
            var acceptHeaders = request.Headers.Accept;
            foreach (var mime in acceptHeaders)
            {
                if (
                    config.Formatters.Any(
                        f => f.SupportedMediaTypes.Any(
                            smt => smt.MediaType.ToLower() == mime.MediaType.ToLower()
                            )
                        )
                    )
                {
                    return true;
                }
            }
            return false;
        }

        public static Dictionary<Type, IHypermediaObjectConfiguration> GetHypermediaObjectConfigurations(
            this HttpConfiguration config)
        {
            if (config.Properties.ContainsKey(HypermediaDefaultConfigurations))
            {
                var configs =
                    config.Properties[HypermediaDefaultConfigurations] as Dictionary<Type, IHypermediaObjectConfiguration>;
                return configs;
            }
            return null;
        }

        public static void AddOrUpdateHypermediaObjectConfiguration(this HttpConfiguration config,
            IHypermediaObjectConfiguration hypermediaDataConfiguration)
        {
            var hypermediaDataConfigurations =
                (config.Properties.ContainsKey(HypermediaDefaultConfigurations) &&
                config.Properties[HypermediaDefaultConfigurations] as Dictionary<Type, IHypermediaObjectConfiguration> != null) ? 
                    config.Properties[HypermediaDefaultConfigurations] as Dictionary<Type, IHypermediaObjectConfiguration> :
                    new Dictionary<Type, IHypermediaObjectConfiguration>();

            if (hypermediaDataConfigurations.ContainsKey(hypermediaDataConfiguration.DtoType))
            {
                hypermediaDataConfigurations[hypermediaDataConfiguration.DtoType] = hypermediaDataConfiguration;
            }
            else
            {
                hypermediaDataConfigurations.Add(hypermediaDataConfiguration.DtoType, hypermediaDataConfiguration);
            }

            config.Properties.AddOrUpdate(HypermediaDefaultConfigurations, hypermediaDataConfigurations,
                (key, oldValue) => hypermediaDataConfigurations);
        }

        public static HypermediaObject GetOverridingHypermediaObjectForContext(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey(OverridingHypermediaObjectForContext))
            {
                return request.Properties[OverridingHypermediaObjectForContext] as HypermediaObject;
            }
            return null;
        }

        public static HypermediaObject GetMergeHypermediaObjectForContext(this HttpRequestMessage request)
        {
            if (request.Properties.ContainsKey(MergeHypermediaObjectConfiguration))
            {
                return request.Properties[MergeHypermediaObjectConfiguration] as HypermediaObject;
            }
            return null;
        }

        /// <summary>
        /// Use this method to store a HypermediaObject object that should replace the default one in the HttpConfiguration.Properties.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="hypermediaData"></param>
        public static void SetOverridingHypermediaObjectForContext(this HttpRequestMessage request, HypermediaObject hypermediaData)
        {
            request.Properties[OverridingHypermediaObjectForContext] = hypermediaData;
        }

        /// <summary>
        /// Use this method to store a HypermediaObject object that should be merged with the default one (if it exists) in the HttpConfiguration.Properties.
        /// </summary>
        /// <param name="request"></param>
        /// <param name="hypermediaData"></param>
        public static void SetMergeHypermediaObjectForContext(this HttpRequestMessage request, HypermediaObject hypermediaData)
        {
            request.Properties[MergeHypermediaObjectConfiguration] = hypermediaData;
        }

        public static HashSet<Link> GetHypermediaContextualLinks(this HttpRequestMessage request)
        {
            var hypermediaContextualLinks = (request.Properties.ContainsKey(HypermediaContextualLinks) && 
                request.Properties[HypermediaContextualLinks] as HashSet<Link> != null) ? 
                request.Properties[HypermediaContextualLinks] as HashSet<Link> : 
                new HashSet<Link>();

            if (request.Properties.ContainsKey(HypermediaContextualLinks))
            {
                request.Properties[HypermediaContextualLinks] = hypermediaContextualLinks;
            }
            else
            {
                request.Properties.Add(HypermediaContextualLinks, hypermediaContextualLinks);
            }

            return hypermediaContextualLinks;
        }
        public static HashSet<Link> GetHypermediaContextualLinks(this ApiController controller)
        {
            var request = controller.Request;

            return GetHypermediaContextualLinks(request);
        }
        public static HashSet<Link> AddOrUpdateHypermediaContextualLinks(this ApiController controller, Link link)
        {
            var hypermediaContextualLinks = GetHypermediaContextualLinks(controller);

            if (hypermediaContextualLinks.Contains(link))
            {
                hypermediaContextualLinks.Remove(link);
            }

            hypermediaContextualLinks.Add(link);

            //if (request.Properties.ContainsKey(HypermediaContextualLinks))
            //{
            //    request.Properties[HypermediaContextualLinks] = hypermediaContextualLinks;
            //}
            //else
            //{
            //    request.Properties.Add(HypermediaContextualLinks, hypermediaContextualLinks);
            //}

            return hypermediaContextualLinks;
        }
        public static HashSet<Link> AddOrUpdateHypermediaContextualLinks(this HashSet<Link> links, Link link)
        {
            if (links.Contains(link))
            {
                links.Remove(link);
            }

            links.Add(link);

            //if (request.Properties.ContainsKey(HypermediaContextualLinks))
            //{
            //    request.Properties[HypermediaContextualLinks] = hypermediaContextualLinks;
            //}
            //else
            //{
            //    request.Properties.Add(HypermediaContextualLinks, hypermediaContextualLinks);
            //}

            return links;
        }
    }
}