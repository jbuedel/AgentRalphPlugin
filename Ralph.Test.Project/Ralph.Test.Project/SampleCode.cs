using System;

// ReSharper disable ConvertToConstant.Local

namespace Ralph.Test.Project
{
    internal class SampleCode
    {
        public void ArbitraryMethod()
        {
            Console.WriteLine("Begin...");
            var start_time = DateTime.Now;
            Console.WriteLine("Middle");
            var run_time = DateTime.Now.Subtract(start_time);
            var msg = string.Format("End.  Ran for {0}s.", run_time.TotalSeconds);
            Console.WriteLine(msg);
        }

        public double Volume()
        {
            Console.WriteLine("Begin volume computation.");
            double height = 8.0, length = 3.0, width = 4.0, scaling = 1.1;
            double area = length*width;
            if (DateTime.Now.Day == 17 && DateTime.Now.Month == 3)
            {
                string msg = "Dude, it's St. Pats.";
                Console.WriteLine(msg);
            }
            double volume = height*area;
            return volume*scaling;
        }
    }
}

// ReSharper restore ConvertToConstant.Local

class Joemama
{
    public void Foo() { int i = 32 + DateTime.Now.Millisecond; string str = "zippy"; Console.Write(str); Console.Write("i=" + i); if (DateTime.Now == DateTime.Today) throw new ApplicationException("That's just crazy."); }
    void NewMethod() { int i = 32 + DateTime.Now.Millisecond; string str = "zippy"; Console.Write(str); Console.Write("i=" + i); if (DateTime.Now == DateTime.Today) throw new ApplicationException("That's just crazy."); }
}