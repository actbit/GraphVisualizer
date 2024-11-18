using System;

namespace GraphLibrary
{
    public abstract class GraphAction
    {
        public virtual Node? Action(Node node)
        {
            return node;
        }

        public static void Print(string message)
        {
            InternalClass.Print("log:"+message);
        }
    }
}
