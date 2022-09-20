﻿using WebAPI.DAO;
using WebAPI.Entity;

namespace WebAPI.Service
{
    public class RelationService
    {
        public static void UpdateRelation(SysUser Staff,
                                          List<CourseOffering> CourseAddList,
                                          List<CourseOffering> CourseRemoveList)
        {
            RelationDAO relationDAO = new();
            relationDAO.Delete(Staff, CourseRemoveList);
            relationDAO.Insert(Staff, CourseAddList);
        }

        public static void UpdateRelation(CourseOffering courseOffering,
                                  List<SysUser> StaffAddList,
                                  List<SysUser> StaffRemoveList)
        {
            RelationDAO relationDAO = new();
            relationDAO.Delete(courseOffering, StaffRemoveList);
            relationDAO.Insert(courseOffering, StaffAddList);
        }
    }
}