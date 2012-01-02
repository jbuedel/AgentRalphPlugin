// NotMatch - Foo & Bar have different parameter names.

using System;

internal class parm
{
    private void Foo(string bar_str)
    {
        bar_str += "zippy";
        Console.Write(bar_str);
    }

    private void Bar(string jibberjab)
    {
        jibberjab += "zippy";
        Console.Write(jibberjab);
    }
}