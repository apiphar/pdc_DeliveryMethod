using TAM.TANGO.Helpers;

namespace TAM.TANGO.Models
{
    public class RoutingGroupSearchParameter : BasicSearchParameters
    {
        public string Query { get; set; }
        public int? RoutingGroupId { get; set; }
    }
}
