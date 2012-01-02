// NotMatch
namespace AgentRalph.RowTestsData
{
    public class Test018
    {
        int Foo()
        {
            if(true)
            {
                return 7;
            }
            else
            {
                return 9;
            }
        }

        int Bar()
        {
            if(true)
            {
                return 9;
            }
            else
            {
                return 7;
            }
        }
    }
}