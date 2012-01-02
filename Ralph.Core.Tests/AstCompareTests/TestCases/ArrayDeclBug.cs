// NotMatch
namespace Ralph.Test.Project
{
    internal class ArrayDeclBug
    {
        public int Foo()
        {
            int[] i_ar;
            return 7;
        }

        public int Bar()
        {
            int[] i_ar = new int[]{7, 8,3 };
            return 7;}
    }
}