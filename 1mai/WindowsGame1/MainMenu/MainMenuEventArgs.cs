using System;

namespace Operation_Cronos
{
    public class MainMenuEventArgs: EventArgs
    {
        private string userName;
        
        /// <summary>
        /// The user to load
        /// </summary>
        public string UserName {
            get { return userName; }
        }

        public MainMenuEventArgs(string user)
        {
            userName = user;
        }

        public MainMenuEventArgs()
        {
        }
    }
}
