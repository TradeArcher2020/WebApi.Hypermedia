using System;
using System.Net.Http;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Web.UI.WebControls;
using WebApi.Hypermedia;

namespace DemoWebApi.Api
{
    public class HypermediaFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext context)
        {
            //var content = context.Response.Content as ObjectContent;
            //var response = new HypermediaObject(content.Value);
            base.OnActionExecuting(context);
        }

        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            var content = context.Response.Content as ObjectContent;
            var request = context.Request;
            var response = context.ActionContext.Response;
            if (content != null)
            {
                //TODO: wrap content with something containing the Hypermedia Data

                var config = response.RequestMessage.GetConfiguration();
                var defaultHypermediaObjectConfigurations = config.GetHypermediaObjectConfigurations();

                //Check if response.RequestMessage.Headers.Accept matches a registered Hypermedia Accept header in registeredHypermediaAcceptFormatters
                if (request.IsAcceptSupported())
                {
                    //Next, check the response.RequestMessage.Properties for an overriding HypermediaObject object.  
                    var overridingHypermediaObject = response.RequestMessage.GetOverridingHypermediaObjectForContext();
                    //If found use instead of the default one found in the configuration.
                    if (overridingHypermediaObject != null)
                    {
                        content.Value = overridingHypermediaObject;
                    }
                    //Get instance and call the factory method passing in the response.Content.
                    //Check defaultHypermediaObjectConfigurations collection for a registered default HypermediaObject object where T is the same type as response.Content
                    //If found, get instance and call the factory method passing in the response.Content.
                    else if (defaultHypermediaObjectConfigurations.ContainsKey(content.ObjectType))
                    {
                        var defaultHypermediaObjectConfiguration = defaultHypermediaObjectConfigurations[content.ObjectType];
                        var defaultHypermediaObject = defaultHypermediaObjectConfiguration.Create(content.Value);

                        //If not found, check response.RequestMessage.Properties for a merge HypermediaObject object.
                        var mergeHypermediaObject = response.RequestMessage.GetMergeHypermediaObjectForContext();
                        //If found, merge the properties in that object with the default one found above.  
                        if (mergeHypermediaObject != null)
                        {
                            //Do merge
                            defaultHypermediaObject.Links.UnionWith(mergeHypermediaObject.Links);
                        }

                        //If the content itself was already a HypermediaObject, then merge it's links with the defaults from the configuration.
                        var contentHypermediaObject = content.Value as IHypermediaObject;
                        if (contentHypermediaObject != null)
                        {
                            defaultHypermediaObject.Links.UnionWith(contentHypermediaObject.Links);
                        }

                        content.Value = defaultHypermediaObject;

                        //NOTE: Once the response.Content is set to an instance of HypermediaObject with link definitions, then the appropriate formatter will use 
                        // that object to serialize the response body using the requested
                    }

                    //Now that the content value is set to a HypermediaObject, we need to process the properties of the encapsulated DTO and map them to HypermediaObjects
                    this.ProcessDtoProperties((IHypermediaObject)content.Value);
                }
            }
            base.OnActionExecuted(context);

        }

        private void ProcessDtoProperties(IHypermediaObject hypermediaObject, string propertyPath = "")
        {
            var dto = hypermediaObject.OriginalDto;

            foreach (var property in dto.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public))
            {
                //propertyPath = property.PropertyType.GetNestedTypes()
                //hypermediaObject.Data.Add();
            }
        }
    }
    //public class HypermediaHandler : DelegatingHandler
    //{
    //    protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    //    {
    //        return base.SendAsync(request, cancellationToken)
    //            .ContinueWith(task =>
    //            {
    //                // work on the response
    //                var response = task.Result;
    //                var content = response.Content as ObjectContent;
    //                if (content != null)
    //                {
    //                    //TODO: wrap content with something containing the HypermediaObject

    //                    var config = response.RequestMessage.GetConfiguration();
    //                    var defaultHypermediaObjectConfigurations = config.GetHypermediaObjectConfigurations();

    //                    //Check if response.RequestMessage.Headers.Accept matches a registered Hypermedia Accept header in registeredHypermediaAcceptFormatters
    //                    if (request.IsAcceptSupported())
    //                    {
    //                        //Next, check the response.RequestMessage.Properties for an overriding HypermediaObject object.  
    //                        var overridingHypermediaObject = response.RequestMessage.GetOverridingHypermediaObjectForContext();
    //                        //If found use instead of the default one found in the configuration.
    //                        if (overridingHypermediaObject != null)
    //                        {
    //                            content.Value = overridingHypermediaObject;
    //                        }
    //                        //Get instance and call the factory method passing in the response.Content.
    //                        //Check defaultHypermediaObjectConfigurations collection for a registered default HypermediaObject object where T is the same type as response.Content
    //                        //If found, get instance and call the factory method passing in the response.Content.
    //                        else if (defaultHypermediaObjectConfigurations.ContainsKey(content.ObjectType))
    //                        {
    //                            var defaultHypermediaObjectConfiguration = defaultHypermediaObjectConfigurations[content.ObjectType];
    //                            var defaultHypermediaObject = defaultHypermediaObjectConfiguration.Create(content.Value);

    //                            //If not found, check response.RequestMessage.Properties for a merge HypermediaObject object.
    //                            var mergeHypermediaObject = response.RequestMessage.GetMergeHypermediaObjectForContext();
    //                            //If found, merge the properties in that object with the default one found above.  
    //                            if (mergeHypermediaObject != null)
    //                            {
    //                                //Do merge
    //                                defaultHypermediaObject.Links.UnionWith(mergeHypermediaObject.Links);
    //                                content.Value = defaultHypermediaObject;
    //                            }

    //                            //NOTE: Once the response.Content is set to an instance of HypermediaObject with link definitions, then the appropriate formatter will use 
    //                            // that object to serialize the response body using the requested
    //                        }
    //                    }

    //                }
    //                return response;
    //            }, cancellationToken);
    //    }
    //}

    //using AppFunc = Func<IDictionary<string, object>, Task>;

    //public class CustomMiddleware
    //{
    //    private readonly AppFunc _next;

    //    public CustomMiddleware(AppFunc next)
    //    {
    //        this._next = next;
    //    }

    //    public async Task Invoke(IDictionary<string, object> env)
    //    {
    //        IOwinContext context = new OwinContext(env);

    //        // Buffer the response
    //        var stream = context.Response.Body;
    //        var buffer = new MemoryStream();
    //        context.Response.Body = buffer;

    //        await this._next(env);

    //        buffer.Seek(0, SeekOrigin.Begin);
    //        var reader = new StreamReader(buffer);
    //        string responseBody = await reader.ReadToEndAsync();

    //        // Now, you can access response body.
    //        Debug.WriteLine(responseBody);

    //        // You need to do this so that the response we buffered
    //        // is flushed out to the client application.
    //        buffer.Seek(0, SeekOrigin.Begin);
    //        await buffer.CopyToAsync(stream);
    //    }
    //}

}