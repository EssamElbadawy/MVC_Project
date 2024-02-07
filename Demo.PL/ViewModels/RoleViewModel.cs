using System;
using System.ComponentModel;

namespace Demo.PL.ViewModels
{
    public class RoleViewModel
    {
        public string Id { get; set; }
        [DisplayName("Role")]
        public string RoleName { get; set; }


        public RoleViewModel()
        {
            Id = Guid.NewGuid().ToString();
        }
    }
}
