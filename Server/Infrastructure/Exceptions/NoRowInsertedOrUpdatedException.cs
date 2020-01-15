using System;
using System.Runtime.Serialization;

namespace Server.Infrastructure.Exceptions
{
    [Serializable]
    public class NoRowInsertedOrUpdatedException : Exception
    {
        public NoRowInsertedOrUpdatedException() : base("No row inserted or updated in database")
        {
        }

        protected NoRowInsertedOrUpdatedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
