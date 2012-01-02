// Match

        using System.Data;

class FooBar006
        {
            private bool Foo( DataSet ds )
            {
                return ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
            }

            private bool Bar( DataSet ds )
            {
                return ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0;
            }
        }
        