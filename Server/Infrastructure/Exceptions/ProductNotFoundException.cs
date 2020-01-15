using System;
using System.Runtime.Serialization;

namespace Server.Infrastructure.Exceptions
{
    [Serializable]
    public class ProductNotFoundException : Exception
    {
        public ProductNotFoundException(string declination, string reference) : base($"No product found for declination/reference couple : '{declination}/{reference}'")
        {
        }

        protected ProductNotFoundException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
