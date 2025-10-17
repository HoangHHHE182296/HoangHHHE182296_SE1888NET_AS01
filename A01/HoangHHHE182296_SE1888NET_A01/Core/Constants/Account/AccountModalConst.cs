using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Constants.Account {
    public class ModalModel {
        public string? Name { get; set; }
        public int Value { get; set; }
        public string? Title { get; set; }
    }

    public static class AccountModalConst {
        public static ModalModel Create = new ModalModel {
            Name = "Create",
            Value = 1,
            Title = "Create A New Account"
        };

        public static ModalModel Edit = new ModalModel {
            Name = "Edit",
            Value = 2,
            Title = "Edit Account"
        };

        public static ModalModel ChangePassword = new ModalModel {
            Name = "Change Password",
            Value = 3,
            Title = "Change Password Account"
        };

        public static ModalModel Delete = new ModalModel {
            Name = "Delete",
            Value = 4,
            Title = "Delete Account"
        };
    }
}
