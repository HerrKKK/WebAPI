﻿/******************************************
  2022 Trimester 3 INFT6900 Final Project
  Team   : Four Square
  Author : Weiran Wang
  Date   : 21/09/2022
******************************************/

using Microsoft.AspNetCore.Mvc;
using WebAPI.DAO;
using WebAPI.Entity;
using WebAPI.Model;

namespace WebAPI.Service
{
    public class CourseService
    {
        public static void Insert(CourseOffering course)
        {
            new CourseOfferingDAO().Insert(course);
        }
        public static void Update(CourseOffering course)
        {
            new CourseOfferingDAO().Update(course);
        }
        public static void Delete(CourseOffering course)
        {
            // all related assessment templates
            AssessmentService.Delete(AssessmentService.QueryTemplates(course));
            new CourseOfferingDAO().Delete(course);
        }
        static public List<CourseOffering> Query(CourseOffering Course,
                                                 SysUser user,
                                                 Assessment assessment)
        {
            /*return new CourseOfferingDAO().TestQuery();*/
            return new CourseOfferingDAO().Query(Course, user, assessment, true);
        }
        static public List<CourseOffering> QueryCandidates(CourseOffering Course,
                                         SysUser user,
                                         Assessment assessment)
        {
            return new CourseOfferingDAO().Query(Course, user, assessment, false);
        }
        static public List<CourseOffering> QueryMultiple(CourseQueryModel model)
        {
            return new CourseOfferingDAO()
                      .QueryMultiple(model.Years,
                                     model.Semesters,
                                     model.Names);
        }
    }
}
