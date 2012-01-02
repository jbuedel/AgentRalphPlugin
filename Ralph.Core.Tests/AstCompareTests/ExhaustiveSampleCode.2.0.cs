using System;

namespace AgentRalph.Tests.AstCompareTests
{
    public class Class1<T> : Base
    {   
        public static void whatevs()
        {
            Class1<int> foo = new Class1<int>();
            foo.Field = 3;
        }

        protected int Field;

        [MyAttrib(Name="Foober")]
        public T Foo()
        {
            using (MyDisposable m = new MyDisposable())
            {
                var v = 1;
                switch (v)
                {
                    case 3:
                        break;
                    case 4:
                        goto case 4;
                }
            }

            this.FooEvent += new Class1<T>.FooDelegate(Class1_FooEvent);
            this.FooEvent -= Class1_FooEvent;

            checked
            {
                
            }

            foreach (var c in "".ToCharArray())
            {
                continue;
            }

            if (true)
            {
                
            }
            else if (true)
            {

            }

            do
            {
                
            } while (false);

            int i = sizeof(int);            

            return default(T);
        }

        void Class1_FooEvent()
        {
            
        }

        public Class1() { }
        public Class1(int i):base() { }
        public Class1(string i):this() { }
        ~Class1() { }
        public delegate void FooDelegate();

        public event FooDelegate FooEvent;

        public int this[int i] { get { return 1; } set {} }


    }

    public class Base
    {
    }

    public class MyAttribAttribute : Attribute
    {
        public string Name
        {
            get { return null; }
            set {  }
        }
    }


    public class MyDisposable : IDisposable
    {
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}