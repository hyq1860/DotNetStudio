using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using NugetMvc5Site.Contracts;

namespace NugetMvc5Site.Implements
{
    public class UserService:IUserService
    {
        public bool Login()
        {
            return true;
        }
    }
}