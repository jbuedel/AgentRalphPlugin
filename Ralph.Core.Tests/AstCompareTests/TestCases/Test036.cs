// Match - Tests TypeReferenceExpressions.
namespace AgentRalph.AstCompareTestsData
{
    public class Test036
    {
        private string Foo(int longIndexName, DataSet ds)
        {
            string Key = ds.Tables[0].Rows[0]["Key"].ToString();

            if (!string.IsNullOrEmpty(Key) && Key != "SAMP")
            {
                try
                {
                    ds.Tables[0].Rows[0]["Key"] = Encoding.ASCII.GetString(PerfFormOp(Convert.FromBase64String(Key), longIndexName));
                    ds.AcceptChanges();
                }
                catch (Exception ex)
                {
                    Logging.Post(ex);
                }
            }

            return ds.GetXml();
        }

        private string Bar(int longIndexName, DataSet ds)
        {
            string Key = ds.Tables[0].Rows[0]["Key"].ToString();

            //ds.Tables[0].Rows[0]["Description"] = "Dude!";

            if (!string.IsNullOrEmpty(Key) && Key != "SAMP")
            {
                try
                {
                    ds.Tables[0].Rows[0]["Key"] = Encoding.ASCII.GetString(PerfFormOp(Convert.FromBase64String(Key), longIndexName));
                    ds.AcceptChanges();
                }
                catch (Exception ex)
                {
                    Logging.Post(ex);
                }
            }

            return ds.GetXml();
        }
    }
}