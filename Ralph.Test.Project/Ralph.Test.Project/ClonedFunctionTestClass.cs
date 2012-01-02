using System;
 using System.Collections;
using System.Collections.Generic;

namespace Ralph.Test.Project
{
    class ClonedFunctionTestClass
    {
        public void Test1()
        {
            if(!Connected())
                throw new Exception();
        }

        public void Test2()
        {
            if (!Connected())
                throw new Exception();
        }

        public void ContainsCloneOfTest1AndTest2()
        {
            Console.WriteLine();
            
            if (!Connected())
                throw new Exception();

            Console.WriteLine();
        }

        public void ContainsCloneOfTest1AndTest2_2()
        {
            if (!Connected())
                throw new Exception();

            Console.WriteLine();
            Console.WriteLine();
        }

        public void ContainsACloneOfCloneWParams()
        {
            int x = 7;
            int y = 8;
            Console.WriteLine(x*y);
            Console.WriteLine(x-y);
        }

        private void CloneWParams(int x, int y)
        {
            Console.WriteLine(x * y);
            Console.WriteLine(x - y);
        }

        private bool Connected()
        {
            throw new NotImplementedException();
        }

        public void Baz(DateTime t, int j)
		{
			Console.Write(8);
			Console.Write(8);
			Console.Write(8);
			Console.Write(8);
			Console.Write(8);
			Console.Write(8);
			Console.Write(8);
			Console.Write(8);
			Console.Write(8);
		}


        public void Bar(DateTime t, int j)
        {
            Console.Write(8);
            Console.Write(8);
            Console.Write(8);
            Console.Write(8);
            Console.Write(8);
            Console.Write(8);
            Console.Write(8);
            Console.Write(8);
            Console.Write(8);
        }
    }
    // ReSharper disable ConvertToConstant.Local
    class TestRenameLocalVar
    {
        private void Foo()
        {
            string foo_str = "zippy";
            Console.Write(foo_str);
        }

        private void Bar()
        {

            string bar_str = "zippy";
            Console.Write(bar_str);
        }
    }

    // ReSharper restore ConvertToConstant.Local
}