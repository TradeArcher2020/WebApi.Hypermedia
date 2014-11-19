using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using WebApi.Hypermedia.Formatters.Hal.JsonConverters;

namespace WebApi.Hypermedia.Formatters
{
    public class JsonHalMediaTypeFormatter : JsonMediaTypeFormatter
    {
        //readonly ResourceListConverter resourceListConverter = new ResourceListConverter();
        //readonly ResourceConverter resourceConverter = new ResourceConverter();
        //readonly LinksConverter linksConverter = new LinksConverter();
        //readonly EmbeddedResourceConverter embeddedResourceConverter = new EmbeddedResourceConverter();
        private readonly JsonHalConverter _jsonConverter = new JsonHalConverter();

        public JsonHalMediaTypeFormatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("application/hal+json"));
            SerializerSettings.Converters.Add(_jsonConverter);
            //SerializerSettings.Converters.Add(linksConverter);
            //SerializerSettings.Converters.Add(resourceListConverter);
            //SerializerSettings.Converters.Add(resourceConverter);
            //SerializerSettings.Converters.Add(embeddedResourceConverter);
            SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
        }

        //public override bool CanReadType(Type type)
        //{
        //    return typeof(HypermediaObject).IsAssignableFrom(type);
        //}

        //public override bool CanWriteType(Type type)
        //{
        //    //serialize any type
        //    return true;
        //    //return typeof(HypermediaObject).IsAssignableFrom(type);
        //}

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger)
        {
            Debug.Write("ReadFromStreamAsync");
            return base.ReadFromStreamAsync(type, readStream, content, formatterLogger);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext)
        {
            Debug.Write("WriteToStreamAsync");
            return base.WriteToStreamAsync(type, value, writeStream, content, transportContext);
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            Debug.Write("GetPerRequestFormatterInstance");
            return base.GetPerRequestFormatterInstance(type, request, mediaType);
        }
    }
}
