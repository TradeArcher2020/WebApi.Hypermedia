using System.Collections.Generic;

namespace WebApi.Hal.Web.Api.Resources
{
    public abstract class PagedRepresentationList<TRepresentation> : SimpleListRepresentation<TRepresentation>
    {
        protected PagedRepresentationList(IList<TRepresentation> res, int totalResults, int totalPages, int page)
            : base(res)
        {
            TotalResults = totalResults;
            TotalPages = totalPages;
            Page = page;
        }

        public int TotalResults { get; set; }
        public int TotalPages { get; set; }
        public int Page { get; set; }

    }
    public abstract class SimpleListRepresentation<TResource>
    {
        protected SimpleListRepresentation()
        {
            ResourceList = new List<TResource>();
        }

        protected SimpleListRepresentation(IList<TResource> list)
        {
            ResourceList = list;
        }

        public IList<TResource> ResourceList { get; set; }
    }
}