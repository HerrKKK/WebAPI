﻿/******************************************
  2022 Trimester 3 INFT6900 Final Project
  Team   : Four Square
  Author : Weiran Wang
  Date   : 19/09/2022
******************************************/

using SqlSugar;
using WebAPI.Entity;
using WebAPI.Service;

namespace WebAPI.DAO
{
    public class PrivilegeDAO
    {
        private SqlSugarClient db;
        public PrivilegeDAO()
        {
            db = UtilService.GetDBClient();
        }

        public int Insert(Privilege Privilege)
        {
            return db.Insertable(Privilege).ExecuteCommand();
        }

        public bool Update(Privilege Privilege)
        {
            return db.Updateable(Privilege)
                     .IgnoreColumns(ignoreAllNullColumns: true)
                     .ExecuteCommandHasChange();
        }
        public List<Privilege> Query(Privilege Privilege,
                                     int SysUserID,
                                     int CourseOfferingID)
        {
            var res = db.Queryable<Privilege>();
            if (Privilege != null && Privilege.PrivilegeName != null)
                res = res.Where(it => it.PrivilegeName == Privilege.PrivilegeName);
            if (SysUserID > 0)
            {
                res = res.Includes(x => x.StaffList)
                         .Where(x => x.StaffList
                         .Any(z => z.SysUserID == SysUserID));
            }
            if (CourseOfferingID > 0)
            {
                res = res.Includes(x => x.CourseOfferingList)
                         .Where(x => x.CourseOfferingList
                         .Any(z => z.CourseOfferingID == CourseOfferingID));
            }

            return res.ToList();
        }
    }
}
