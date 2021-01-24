using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Threading;
using Microsoft.ML;
using Efflux.Data;

namespace Efflux.Analytics
{
    public class TopicMLDataView<T> : TopicDataView, IDataView
    {
        readonly DataViewSchema schema;

        public TopicMLDataView(ITopicConsumer consumer, DataSchema schema) : base(consumer, schema)
        {
            // TODO: DataSchema -> DataViewSchema
            //var b = new DataViewSchema.Builder();
            //b.AddColumn("active", BooleanDataViewType.Instance);
            //b.AddColumn("name", TextDataViewType.Instance);
            //schema = b.ToSchema();
        }

        public DataViewSchema Schema => schema;

        public bool CanShuffle => throw new NotImplementedException();

        public long? GetRowCount()
        {
            return null;
        }

        public DataViewRowCursor GetRowCursor(IEnumerable<DataViewSchema.Column> columnsNeeded, Random rand = null)
        {
            //TODO: handle parameters
            return new TopicDataCursor(this.GetAsyncEnumerator() as TopicDataEnumerator, schema);
        }

        public DataViewRowCursor[] GetRowCursorSet(IEnumerable<DataViewSchema.Column> columnsNeeded, int n, Random rand = null)
        {
            //TODO: handle parameters
            return new DataViewRowCursor[] { new TopicDataCursor(this.GetAsyncEnumerator() as TopicDataEnumerator, schema) };
        }

    }
}
