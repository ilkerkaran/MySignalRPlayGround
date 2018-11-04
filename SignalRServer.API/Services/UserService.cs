using System;
using System.Collections.Generic;
using System.Linq;

namespace SignalRServer.API.Services
{
    public class UserService
    {
        private List<string> loggedInUsers;

        public UserService()
        {
            this.loggedInUsers = new List<string>();
        }

        public bool IsAuthorized(string username, string password)
        {
            return true;
            if (loggedInUsers.Any(u => string.Equals(u, username, StringComparison.InvariantCultureIgnoreCase)))
                return false;
            this.loggedInUsers.Add(username);
            return true;
        }
    }
}