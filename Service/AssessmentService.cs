﻿using System.Diagnostics;
using WebAPI.DAO;
using WebAPI.Entity;

namespace WebAPI.Service
{
    public class AssessmentService
    {
        private AssessmentService()
        {
            UserDAO = new();
            CourseOfferingDAO = new();
            AssessmentDAO = new();
        }
        private static AssessmentService Instance = new();
        public static AssessmentService GetInstance()
        {
            Instance ??= new AssessmentService();
            return Instance;
        }

        private readonly UserDAO UserDAO;
        private readonly CourseOfferingDAO CourseOfferingDAO;
        private readonly AssessmentDAO AssessmentDAO;
        public void Insert(AssessmentTemplate template)
        {
            var course = new CourseOffering() { CourseOfferingID = template.CourseOfferingID};

            var studentList = UserDAO
                                 .QueryUsers(new SysUser()
                                 {
                                     Permission = 1,
                                 },
                                 course,
                                 true);
            course = CourseOfferingDAO.Query(course).First();
            template.AssessmentID = Guid.NewGuid().ToString();
            template.CourseOfferingName = course.CourseName + " "
                + course.Semester + " " + course.Year;

            var instanceList = new List<AssessmentInstance>();
            foreach (SysUser student in studentList)
            {
                instanceList.Add(new AssessmentInstance(template, student));
            }
            AssessmentDAO.Insert(new List<AssessmentTemplate>() {template},
                                                    instanceList);
        }
        public void Update(Assessment assessment)
        {
            if (assessment is AssessmentTemplate) {
                var instanceList = AssessmentDAO.Query(null, assessment);
                foreach (var instance in instanceList)
                {
                    instance.Name = assessment.Name;
                    instance.Type = assessment.Type;
                    instance.BeginDate = assessment.BeginDate;
                    instance.EndDate = assessment.EndDate;
                    instance.Location = assessment.Location;
                }
                AssessmentDAO.Update(null, instanceList);
            }

            AssessmentDAO.Update(assessment);
        }
        public List<AssessmentInstance> Query(AssessmentInstance instance)
        {
            return AssessmentDAO.Query(instance);
        }
        public List<AssessmentTemplate> QueryTemplates(CourseOffering course)
        {
            return AssessmentDAO.Query(null, course);
        }
        public List<AssessmentInstance> QueryInstance(SysUser student)
        {
            student.SysUserID = UserDAO.QueryUsers(student, null, true).First().SysUserID;
            return AssessmentDAO.Query(student, null);
        }
        public void Delete(List<AssessmentTemplate> templates)
        {
            var instanceList = new List<AssessmentInstance>();
            
            foreach (var template in templates)
            {
                instanceList.AddRange(AssessmentDAO.Query(null, template));
            }

            Debug.WriteLine("delete");
            foreach(var instance in instanceList)
            {
                Debug.WriteLine(instance.Name);
            }

            AssessmentDAO.Delete(templates, instanceList);
        }
        public void Attach(CourseOffering course,
                                  SysUser student)
        {
            var instances = new List<AssessmentInstance>();
            var templates = QueryTemplates(course);
            foreach (var template in templates)
            {
                instances.Add(new AssessmentInstance(template, student));
            }
            AssessmentDAO.Insert(null, instances);
        }
        public void Detach(CourseOffering course,
                                  SysUser students)
        {
            var instances = QueryInstance(students);
            var forDelete = new List<AssessmentInstance>();

            foreach (var instance in instances)
            {
                if (instance.CourseOfferingID == course.CourseOfferingID)
                {
                    forDelete.Add(instance);
                }
            }

            AssessmentDAO.Delete(null, instances);
        }
    }
}
