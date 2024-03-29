﻿/******************************************
  2022 Trimester 3 INFT6900 Final Project
  Team   : Four Square
  Author : Weiran Wang
  Date   : 17/09/2022
******************************************/

using SqlSugar;
using System.Security.Principal;

namespace WebAPI.Entity
{
    public class SysUser
    {
        [SugarColumn(IsIdentity = true, IsPrimaryKey = true)]
        public int SysUserID { get; set; }
        public string PasswordHash { get; set; }
        public string? Salt { get; set; }
        /*
         -1: invalid, 0: Student, 1: staff, 2: Admin, 3: Super User
         */
        public int Permission { get; set; }

        public string? MemberNumber { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }

        public string? Birthdate { get; set; }
        public string? AddressLine1 { get; set; }
        public string? AddressLine2 { get; set; }
        public string? AddressLine3 { get; set; }
        public string? Phone { get; set; }
        public string? Email { get; set; }
    }
}
