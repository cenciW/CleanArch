using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchMvc.Domain.Account
{
    interface ISeedUserRoleInitial
    {
        //Adiconando usuários iniciais...
        void SeedUsers();
        void SeedRoles();
    }
}
