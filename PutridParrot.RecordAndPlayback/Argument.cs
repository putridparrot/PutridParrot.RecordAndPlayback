using System;

namespace PutridParrot.RecordAndPlayback
{
    public class Argument
    {
        public Argument(Type type, object value)
        {
            Type = type;
            Value = value;
        }

        public Type Type { get; }
        public object Value { get; }
    }

}
