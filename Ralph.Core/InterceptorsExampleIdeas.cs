using System;
/* Experimenting with using C# generics in interceptors. */
namespace AgentRalph.CloneCandidateDetectionTestData
{
    public enum Pos { First, Second }
    public class InterceptorsExampleIdeas
    {
        public void Foo(Pos pos)
        {
            if(pos.ToString() == "First")
            {
                
            }
        }

        public void Template1(Pos pos)
        {
            if (pos.ToString() == "First")
            {

            }

            Bar(() => Console.Write(pos.ToString() == "First"));
        }

        private void Bar(Action action)
        {
            action();
        }
    }
}