using System;

public class ExpectedInspectorReferenceException : NullReferenceException
{
    public ExpectedInspectorReferenceException()
    {
    }

    public ExpectedInspectorReferenceException(string message)
        : base(message)
    {
    }
}