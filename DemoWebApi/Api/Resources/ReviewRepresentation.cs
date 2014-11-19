namespace WebApi.Hal.Web.Api.Resources
{
    public class ReviewRepresentation
    {
        public int Id { get; set; }
        public int Beer_Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
    }
}