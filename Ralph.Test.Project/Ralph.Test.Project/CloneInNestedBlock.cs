using System;
namespace Ralph.Test.Project
{
    public class CloneInIfBlock
    {
        // Also tests that a method body w/ a single child still recurses.
        private void Foo()
        {
            if (DateTime.Now.Day == 3)
            {
                Console.WriteLine(7);
            }
        }

        private void Bar()
        {
            Console.WriteLine(7);
        }
    }

    public class CloneInForeachBlock
    {
        void Foo()
        {
            foreach (int i in new int[] { })
            {
                Console.WriteLine(7);
            }
        }

        private void Bar()
        {
            Console.WriteLine(7);
        }
    }
    public class CloneInForBlock
    {
        void Foo()
        {
            for (int i = 0; i < 10; i++)
            {
                    Console.WriteLine(7);
            }
        }

        private void Bar()
        {
            Console.WriteLine(7);
        }
    }
    public class CloneInWhileBlock
    {
        void Foo()
        {
            while (DateTime.Now < DateTime.Today)
            {
                Console.WriteLine(7);
            }
        }

        private void Bar()
        {
            Console.WriteLine(7);
        }
    }
    public class CloneInDoWhileBlock
    {
        void Foo()
        {
            do
            {
                Console.WriteLine(7);
            }
            while (DateTime.Now < DateTime.Today);
        }

        private void Bar()
        {
            Console.WriteLine(7);
        }
    }

    public class CloneInSwitchCaseBlock
    {
        void Foo()
        {
            switch (DateTime.Today.Hour)
            {
                case 1:
                    Console.WriteLine(7);
                    break;
            }
        }

        private void Bar()
        {
            Console.WriteLine(7);
        }
    }

    public class ForEachDoesNotCancelOutAllOtherClones
    {
        void Foo()
        {
            Console.WriteLine(7);

            foreach (int i in new int[] { })
            {
                Console.WriteLine(7);
            }
        }

        private void Bar()
        {
            Console.WriteLine(7);
        }
    }
}