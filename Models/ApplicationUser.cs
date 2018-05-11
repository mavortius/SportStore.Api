using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace SportStore.Api.Models
{
    public class ApplicationUser : IdentityUser
    {
        public ApplicationUser()
        {
        }
        
        public virtual List<Token> Tokens { get; set; }
    }
}