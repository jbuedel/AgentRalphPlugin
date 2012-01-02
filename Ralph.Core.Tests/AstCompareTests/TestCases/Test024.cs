// NotMatch - Foo & Bar have different return types, though their bodies are identical.

namespace AgentRalph.RowTestsData
{
    public class Test024
    {
        double Foo()
        {
            return 7;
        }
        int Bar()
        {
            return 7;
        }
    }
}