using System;
using System.Collections;
using System.Linq;

namespace PutridParrot.RecordAndPlayback
{
    /// <summary>
    /// Tracks the invocation details which include the
    /// Assembly.Type.MethodName and the arguments
    /// passed, including the types, also the result.
    /// </summary>
    public class Invocation
    {
        public Invocation(string name, Argument[] arguments, object result)
        {
            Name = name;
            Arguments = arguments;
            Result = result;
        }

        public string Name { get; set; }
        public Argument[] Arguments { get; set; }
        public object Result { get; set; }

        public bool Equals(Invocation invocation)
        {
            var found = false;
            if (Arguments.Length == invocation.Arguments.Length)
            {
                found = !Arguments
                    .Where((t, i) => !Compare(t.Value, invocation.Arguments[i].Value))
                    .Any();
            }
            return found;
        }

        private bool Compare(object valueA, object valueB)
        {
            return Object.Equals(valueA, valueB);
        }
    }
}
