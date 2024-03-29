﻿/******************************************
  2022 Trimester 3 INFT6900 Final Project
  Team   : Four Square
  Author : Chenrui Zhang
  Date   : 23/09/2022
******************************************/

using WebAPI.Entity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Service;
using System.Dynamic;
using Microsoft.AspNetCore.Http;
using WebAPI.Model;
using SqlSugar.Extensions;
using System.Diagnostics;
using System.Security.Cryptography;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AssessmentController : ControllerBase
    {
        // New template
        [HttpPost]
        public ResponseModel New([FromBody] AssessmentTemplate template)
        {
            try
            {
                AuthenticationService.Authorization(HttpContext.Session, 3);
                AssessmentService.Insert(template);
                return new SuccessResponseModel()
                {
                    Message = "insert a course template",
                };
            }
            catch (Exception e)
            {
                return new FailureResponseModel()
                {
                    Message = e.Message,
                };
            }
        }
        [HttpGet]
        public ResponseModel GetInstance([FromQuery] AssessmentInstance instance)
        {
            try
            {
                return new SuccessResponseModel()
                {
                    obj = AssessmentService.Query(instance),
                    Message = "Got a course template",
                };
            }
            catch (Exception e)
            {
                return new FailureResponseModel()
                {
                    Message = e.Message,
                };
            }
        }
        [HttpGet]
        public ResponseModel Get([FromQuery] CourseOffering course,
                                 [FromQuery] SysUser student)
        {
            try
            {
                if (course.CourseOfferingID != 0)
                {
                    Debug.WriteLine(course.CourseOfferingID);
                    return new SuccessResponseModel()
                    {
                        obj = AssessmentService.QueryTemplates(course),
                        Message = "Got a course template",
                    };
                }
                if (student.UserNumber != null)
                {
                    return new SuccessResponseModel()
                    {
                        obj = AssessmentService.QueryInstance(student),
                        Message = "Got a course instance",
                    };
                }
                return new SuccessResponseModel();
            }
            catch (Exception e)
            {
                return new FailureResponseModel()
                {
                    Message = e.Message,
                };
            }
        }
        [HttpPost]
        public ResponseModel UpdateTemplate([FromBody] AssessmentTemplate assessment)
        {
            try
            {
                AuthenticationService.Authorization(HttpContext.Session, 3);

                AssessmentService.Update(assessment);
                return new SuccessResponseModel();
            }
            catch (Exception e)
            {
                return new FailureResponseModel()
                {
                    Message = e.Message,
                };
            }
        }
        [HttpPost]
        public ResponseModel UpdateInstance([FromBody] AssessmentInstance assessment)
        {
            try
            {
                AuthenticationService.Authorization(HttpContext.Session, 2);

                AssessmentService.Update(assessment);
                return new SuccessResponseModel();
            }
            catch (Exception e)
            {
                return new FailureResponseModel()
                {
                    Message = e.Message,
                };
            }
        }

        [HttpDelete]
        public ResponseModel Delete([FromBody] AssessmentTemplate template)
        {
            try
            {
                AuthenticationService.Authorization(HttpContext.Session, 3);
                AssessmentService.Delete(
                    new List<AssessmentTemplate>() { template });
                return new SuccessResponseModel();
            }
            catch (Exception e)
            {
                return new FailureResponseModel()
                {
                    Message = e.Message,
                };
            }
        }
    }
}
