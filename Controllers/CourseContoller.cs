﻿using WebAPI.Entity;
using Microsoft.AspNetCore.Mvc;
using WebAPI.Service;
using WebAPI.Model;
using System.Diagnostics;
using Microsoft.AspNet.Identity;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class CourseController : ControllerBase
    {
        [HttpGet]
        public ResponseModel Get([FromQuery] CourseOffering Course,
                                 [FromQuery] SysUser user,
                                 [FromQuery] Assessment assessment)
        {
            try
            {
                return new SuccessResponseModel()
                {
                    Message = "Success",
                    obj = CourseService.Query(Course, user, assessment),
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
        [HttpPost]
        public ResponseModel GetMultiple(CourseQueryModel model)
        {
            Debug.WriteLine(model.Years.Count);
            try
            {
                return new SuccessResponseModel()
                {
                    Message = "Success",
                    obj = CourseService.QueryMultiple(model),
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
    }
}
