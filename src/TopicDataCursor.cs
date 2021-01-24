using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.ML;
using Microsoft.ML.Data;
using Efflux.Data;

namespace Efflux.Analytics
{
    public class TopicDataCursor : DataViewRowCursor
    {
        readonly TopicDataEnumerator reader;
        readonly DataViewSchema schema;

        public TopicDataCursor(TopicDataEnumerator reader, DataViewSchema schema)
        {
            this.schema = schema;
            this.reader = reader;
        }

        public override long Position => reader.Position;

        public override long Batch => 0;

        public override DataViewSchema Schema => schema;
  
        public override ValueGetter<TValue> GetGetter<TValue>(DataViewSchema.Column column)
        {
            throw new NotImplementedException();
        }

        public override ValueGetter<DataViewRowId> GetIdGetter()
        {
            throw new NotImplementedException();
        }

        public override bool IsColumnActive(DataViewSchema.Column column)
        {
            throw new NotImplementedException();
        }

        public override bool MoveNext()
        {
            var result = reader.MoveNextAsync().AsTask();
            result.Wait();
            return result.Result;
        }
    }
}
