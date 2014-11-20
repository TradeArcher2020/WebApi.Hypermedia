WebApi.Hypermedia
=================
This project was inspired by the WebApi.Hal (https://github.com/JakeGinnivan/WebApi.Hal) opensource project.  

### The problem this project is trying to solve here is:

Given an ASP.NET Web Api 2.x project...

1. I want to be able to return just a plain old DTO from my controller action.
2. I want to have some default metadata configured that is associated with my DTO.
3. I want to be able to add/modify the metadata associated with my DTO contextually in my controller action.
4. I want to be able to plug-in different Formatters to support different Hypermedia formats over time.
5. I only want to return hypermedia format (with metadata) if the Accept header matches a registered formatter. Otherwise, I want the plain old DTO returned.

A custom `ActionFilterAttribute` (`HypermediaFilter`) can solve # 1 by intercepting the `Request` and replacing it with a `HypermediaObject`.

A configuration registration mechanism similar to the appenders in WebApi.Hal can solve # 2.

Storing extra Links metadata in the `Request.Properties` (made easier with extension methods) can solve # 3. Then, the custom `ActionFilterAttribute` can merge the default metadata with the contextual metadata from the `Request` when it encapsulates the DTO in the `HypermediaObject`.

By keeping the metadata fairly generic and not tying it directly to HAL or any other specific Hypermedia format standard, we can support # 4.

By checking the Accept header inside the custom `ActionFilterAttribute`, we can support # 5.
