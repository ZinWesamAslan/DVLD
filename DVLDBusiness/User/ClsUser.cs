using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLDdataAccess;
using DVLDdataAccess.Users;

namespace DVLDLogic.User
{
    public class ClsUser
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int UserID { set; get; }
        public int PersonID { set; get; }
        public ClsPerson Person { set; get; }
        public string UserName { set; get; }
        public string Password { set; get; }
        public bool IsActive { set; get; }

        public ClsUser()
        {
            this.UserID = -1;
            this.PersonID = -1;
            this.UserName = "";
            this.Password = "";
            this.IsActive = false;

            Mode = enMode.AddNew;
        }

        private ClsUser(int UserID, int PersonID, string UserName, string Password, bool IsActive)
        {
            this.UserID = UserID;
            this.PersonID = PersonID;
            this.Person = ClsPerson.Find(PersonID);
            this.UserName = UserName;
            this.Password = Password;
            this.IsActive = IsActive;

            Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            this.UserID = ClsUsersAccess.AddNewUser(
                this.PersonID,
                this.UserName,
                this.Password,
                this.IsActive
            );

            return (this.UserID != -1);
        }

        private bool _UpdateUser()
        {
            return ClsUsersAccess.UpdateUser(
                this.UserID,
                this.PersonID,
                this.UserName,
                this.Password,
                this.IsActive
            );
        }

        public static ClsUser Find(int UserID)
        {
            int PersonID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;

            if (ClsUsersAccess.GetUserByID(UserID, ref PersonID, ref UserName, ref Password, ref IsActive))
            {
                return new ClsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            else
                return null;
        }

        public static ClsUser Find(string UserName)
        {
            int PersonID = -1;
            int UserID = -1; string Password = "";
            bool IsActive = false;

            if (ClsUsersAccess.GetUserByName(UserName,ref UserID, ref PersonID, ref Password, ref IsActive))
            {
                return new ClsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            else
                return null;
        }


        // marked to delete
        public static ClsUser FindByPersonID(int PersonID)
        {
            int UserID = -1;
            string UserName = "", Password = "";
            bool IsActive = false;

            if (ClsUsersAccess.GetUserByPersonID(PersonID, ref UserID, ref UserName, ref Password, ref IsActive))
            {
                return new ClsUser(UserID, PersonID, UserName, Password, IsActive);
            }
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                        return false;

                case enMode.Update:
                    return _UpdateUser();
            }

            return false;
        }

        public static DataTable GetAllUsers()
        {
            return ClsUsersAccess.GetAllUsers();
        }

        public static bool DeleteUser(int UserID)
        {
            return ClsUsersAccess.DeleteUser(UserID);
        }

        public static bool IsUserExist(int UserID)
        {
            return ClsUsersAccess.IsUserExist(UserID);
        }

        public static bool IsUserExistByPersonID(int PersonID)
        {
            return ClsUsersAccess.IsUserExistByPersonID(PersonID);
        }

        public static bool IsUserExist(string UserName)
        {
            return ClsUsersAccess.IsUserExist(UserName);
        }

        // marked to delete
        public static bool IsUserExist(string UserName, string Password)
        {
            return ClsUsersAccess.IsUserExist(UserName , Password);
        }

        public static bool IsUserActive(string UserName)
        {
            return ClsUsersAccess.IsUserActive(UserName);
        }

        // marked to delete
        public static string GetHashedPassword(string UserName)
        {
            string HashedPassword = "";
            if( ClsUsersAccess.GetHashedPassword(UserName,ref HashedPassword))
            {
                return HashedPassword;
            }
            return null;
        }

        public bool ChangePassword(string NewPassword)
        {
            return ClsUsersAccess.ChangePassword(this.UserID , NewPassword);
        }


    }
}
