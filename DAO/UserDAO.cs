﻿using SqlSugar;
using WebAPI.Entity;
using WebAPI.Model;

namespace WebAPI.DAO
{
    public class UserDAO
    {
        private SqlSugarClient db;
        public UserDAO()
        {
            db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = SysConfigModel.
                                   Configuration.
                                   GetConnectionString("DefaultConnection"),
                DbType = DbType.SqlServer,
                IsAutoCloseConnection = true,
                InitKeyType = InitKeyType.Attribute
            });
            db.CodeFirst.SetStringDefaultLength(200).InitTables(typeof(SysUser));
        }

        public int Insert(SysUser user)
        {
            int id = db.Insertable(user).ExecuteCommand();
            return id;
        }

        public List<SysUser> QueryUserByEmail(String Email)
        {
            return db.Queryable<SysUser>().Where(it => it.Email == Email).ToList();
        }

        public List<SysUser> QueryUsers(PrivateInfoModel info)
        {
            var res = db.Queryable<SysUser>().
                         Where(it => it.Permission == info.Permission);
            if (info.Email != null)
            {
                res = res.Where(it => it.Email == info.Email);
            }
            if (info.UserNumber != null)
            {
                res = res.Where(it => it.UserNumber == info.UserNumber);
            }
            if (info.UserName[0] != null)
            {
                res = res.Where(it => it.UserName[0].Contains(info.UserName[0]));
            }
            if (info.UserName[1] != null)
            {
                res = res.Where(it => it.UserName[1].Contains(info.UserName[1]));
            }
            if (info.Academic != null)
            {
                res = res.Where(it => it.Academic == info.Academic);
            }

            return res.IgnoreColumns(it => it.PasswordHash).
                       IgnoreColumns(it => it.SysUserID).
                       IgnoreColumns(it => it.PrivilegeList).
                       IgnoreColumns(it => it.CourseOfferingList).
                       IgnoreColumns(it => it.Salt).ToList();
        }

        public bool UpdateUser(SysUser UserInfo)
        {
            UserInfo.SysUserID = db.Queryable<SysUser>()
                       .Select(it => it.SysUserID)
                       .First();
            var res = db.Updateable(UserInfo);
            if (UserInfo.Addresses[0] == null
             && UserInfo.Addresses[1] == null
             && UserInfo.Addresses[2] == null)
            {
                res = res.IgnoreColumns(it => new {it.Addresses});
            }
            if (UserInfo.UserName[0] == null
             && UserInfo.UserName[1] == null
             && UserInfo.UserName[2] == null)
            {
                res = res.IgnoreColumns(it => new { it.UserName });
            }

            return res.IgnoreColumns(ignoreAllNullColumns: true)
                      .ExecuteCommandHasChange();
        }

        public bool DeleteUser(String Email)
        {
            SysUser UserInfo = db.Queryable<SysUser>()
                              .First(it => it.Email == Email);
            UserInfo.Permission = 0;

            return db.Updateable(UserInfo)
                        .IgnoreColumns(ignoreAllNullColumns: true)
                        .ExecuteCommandHasChange();
        }
    }
}
