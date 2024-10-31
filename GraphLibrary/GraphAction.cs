using System;

namespace GraphLibrary
{
    public abstract class GraphAction
    {
        public virtual void Action(Node node)
        {

        }

        public static void NextWait()
        {
            InternalClass.WaitNext();
        }
        public static void Print(string message)
        {
            InternalClass.Print(message);
        }
    }
}
