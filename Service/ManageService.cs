﻿using WebAPI.DAO;
using WebAPI.Entity;
using WebAPI.Model;

namespace WebAPI.Service
{
    public class ManageService
    {
        public static int AddUser(SysUser NewUser)
        {
            if (NewUser.Permission < 0 || !UtilService.IsValidEmail(NewUser.Email))
            {
                throw new Exception("invalid email");
            }

            NewUser.Salt = SecurityService.GenerateSalt();
            NewUser.PasswordHash = SecurityService.GetMD5Hash(NewUser.Salt + NewUser.PasswordHash);

            UserDAO userDAO = new();
            return userDAO.Insert(NewUser);
        }

        public static List<SysUser> QueryUsers(SysUser user, CourseOffering course)
        {
            return new UserDAO().QueryUsers(user, course, true);
        }
        public static List<SysUser> QueryUsers(PrivateInfoModel PrivateInfo)
        {
            CourseOffering nullCourse = new();
            return new UserDAO().QueryUsers(PrivateInfo, nullCourse, true);
        }
        public static List<SysUser> QueryCandidateUsers(PrivateInfoModel user,
                                                        CourseOffering course)
        {
            return new UserDAO().QueryUsers(user, course, false);
        }
        public static bool UpdateUser(SysUser UserInfo)
        {
            return new UserDAO().UpdateUser(UserInfo);
        }
        public static void DeleteUser(string userNumber)
        {
            new UserDAO().DeleteUser(userNumber);
        }
    }
}
