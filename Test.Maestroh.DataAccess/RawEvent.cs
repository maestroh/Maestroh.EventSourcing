using System;

namespace Maestroh.DataAccess.Tests
{
    public class RawEvent
    {
        public Guid AggregateId { get; set; }
        public Byte[] Data { get; set; }
        public int Version { get; set; }

    }
}