using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Diff_Service
{
    public enum DiffResultType { Equals, SizeDoesNotMatch, ContentDoesNotMatch };

    public struct Diff
    {
        public int length;
        public int offset;
    }

    [DataContract]
    public class OutputData
    {
        [DataMember (Order = 0)]
        public string ResultType { get; set; }
        [DataMember (Order = 1, EmitDefaultValue = false)]
        public List<Diff> Diffs { get; set; }
    }
}
