using System.ComponentModel.DataAnnotations;

namespace aspnet_webapi
{
    public class BaseOptions
    {
        public string S { get; set; }

        // Validation only hits when first hit on injection
        [Range(0,1000,ErrorMessage="Value for {0} must be between {1} and {2}")]
        public int I { get; set; }

        public override string ToString()
        {
            return $"{this.GetType().Name} S: {S} I: {I}";
        }
    }
}
