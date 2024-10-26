﻿using PAWS_NDV_PetLovers.Models.Records;

namespace PAWS_NDV_PetLovers.ViewModels
{
    public class UserAccountVm
    {
        public UserAccount? userAccount { get; set; }
        public IEnumerable<UserAccount> IuserAccount {  get; set; } 
        public AccountTab activeAccountTab {  get; set; }


        public bool isNull { get; set; }
    }

    public enum AccountTab
    {
        accountList,
        createStaff
    }
}
