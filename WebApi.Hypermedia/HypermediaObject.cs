using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace WebApi.Hypermedia
{
    public interface IHypermediaObject
    {
        HashSet<Link> Medadata { get; set; }

        object OriginalDto { get; }

        Dictionary<object, IHypermediaObject> NestedMedadata { get; set; }
    }


    public class HypermediaObject : IHypermediaObject
    {
        private HashSet<Link> _medadata = new HashSet<Link>();
        private object _originalDto;
        private Dictionary<object, IHypermediaObject> _nestedMetadata = new Dictionary<object, IHypermediaObject>();

        public HypermediaObject()
        {
            
        }
        public HypermediaObject(object dto)
        {
            _originalDto = dto;

            //Set Links based on Config
            //this.GetDefaultConfigForThisObject(dto);
        }

        public HypermediaObject(string selfUri, object dto) : this(dto)
        {
            _medadata.Add(new SelfLink(selfUri));
        }


        public object OriginalDto
        {
            get { return _originalDto; }
            set { _originalDto = value; }
        }

        //Metadata for this object level (not sub-properties)
        public HashSet<Link> Medadata
        {
            get { return _medadata; }
            set { _medadata = value; }
        }

        //Sub-properties from OriginalDto converted into other HypermediaObject's
        public Dictionary<object, IHypermediaObject> NestedMedadata
        {
            get { return _nestedMetadata; }
            set { _nestedMetadata = value; }
        }

        //example: OriginalDto is a Customer Object with sub-Property of type Address:
        /*
         * {
         *  "Name": "Chris Swain",
         *  "Address": {
         *      "Street": "12345 Street Ln.",
         *      "City": "Austin",
         *      "State": "Texas",
         *      "Zip": "78751",
         *  }
         * }
         * 
         * Therefore, the Data property would contain:
         * "Name": "Chris Swain"
         * "Address": HypermediaObject
         * 
         * And the Address HypermediaObject would have the Address stored in the OriginalDto with any
         *  metadata we want to add for the Address object
         * 
         * This allows us to store object Hypermedia metadata for each object and sub-property individually.
         */
    }

    //Example configuration

    //This is just a simple example UserDto object
    //public class UserDto
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //}

    //This is an example implementation of the abstract HypermediaObjectConfigurationBase class
    //public class UserDtoHypermediaObjectConfiguration : HypermediaObjectConfigurationBase<UserDto>
    //{
    //    public override void Configure()
    //    {
    //        Links.Add(new SelfLink("/api/Users/{id}"));  //where {id} represents the value of the Id property on the UserDto
    //    }
    //}

    //This is an example of how to register a custom HypermediaObjectConfiguration with the Web Api configuration.
    //public static class WebApiConfig
    //{
    //    public static void Register(HttpConfiguration config)
    //    {
    //        config.AddOrUpdateHypermediaObjectConfiguration(new UserDtoHypermediaObjectConfiguration());
    //    }
    //}
}
