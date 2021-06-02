using System;

namespace MiniRPG.Navigation
{
    public class NavigationException : Exception
    {
        public NavigationException(string message) : base(message)
        {
            
        }
    }
}