// Match
using System;

class FooBar002
{
    private void Foo()
    {
        if (!IsConnected())
            throw new DatabaseConnectionException();
    }

    private void Bar()
    {
        if (!IsConnected())
            throw new DatabaseConnectionException();
    }

    private class DatabaseConnectionException : Exception
    {
    }

    private bool IsConnected()
    {
        return true;
    }
}
        