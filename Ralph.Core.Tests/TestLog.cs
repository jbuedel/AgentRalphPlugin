using System.Diagnostics;

namespace AgentRalph.Tests
{
    public static class TestLog
    {
        public static void EmbedPlainText(string name, string text)
        {
            Debug.WriteLine(name);
            Debug.WriteLine(text);
        }

        public static void EmbedPlainText(string text)
        {
            Debug.WriteLine(text);
        }
    }
}