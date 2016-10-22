using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace Nancy.Metadata.Swagger.DemoApplication.Model
{
    public class GenericResponseModel<T> where T : class
    {
        [Required]
        public T Child { get; set; }
    }
}
