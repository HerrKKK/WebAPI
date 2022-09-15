﻿using SqlSugar;

namespace WebAPI.Entity
{
    public class Privilege
    {
        public Privilege()
        {
            PrivilegeName = "Defaule Privilege Name";
        }

        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int PrivilegeID { get; set; }
        [SugarColumn(ColumnDataType = "varchar(100)")]
        public string PrivilegeName { get; set; }
        [SugarColumn(ColumnDataType = "varchar(1000)", IsNullable = true)]
        public string? Description { get; set; }

        [Navigate(typeof(PrivilegeOfferingMapping),
                  nameof(PrivilegeOfferingMapping.PrivilegeID),
                  nameof(PrivilegeOfferingMapping.CourseOfferingID))]
        public List<CourseOffering>? CourseOfferingList { get; set; }

        [Navigate(typeof(PrivilegeStaffMapping),
          nameof(PrivilegeStaffMapping.PrivilegeID),
          nameof(PrivilegeStaffMapping.SysUserID))]
        public List<Staff>? StaffList { get; set; }
    }
}
