﻿/******************************************
  2022 Trimester 3 INFT6900 Final Project
  Team   : Four Square
  Author : Chenrui Zhang
  Date   : 01/11/2022
******************************************/

using K4os.Hash.xxHash;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Http.Headers;
using WebAPI.Entity;
using WebAPI.Model;
using WebAPI.Service;
using static System.Net.Mime.MediaTypeNames;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class DocumentController : ControllerBase
    {
        [HttpPost]
        public ResponseModel Upload([FromForm(Name="file")] List<IFormFile> files,
                                    [FromForm(Name="ApplicationID")] long applicationID,
                                    [FromForm(Name="Type")] string type)
        {
            try
            {
                SessionService.isOwner(HttpContext.Session, new Entity.Application()
                {
                    ApplicationID = applicationID
                });

                DocumentService
                    .GetInstance()
                    .AddDocuments(files,
                                  applicationID,
                                  type);
                return new SuccessResponseModel();
            }
            catch (Exception e)
            {
                return new FailureResponseModel()
                {
                    Message = e.Message
                };
            }
        }

        [HttpDelete]
        public ResponseModel Delete([FromBody] Document document)
        {
            try
            {
                DocumentService
                   .GetInstance().DeleteDocument(document.DocumentID);
                return new SuccessResponseModel();
            }
            catch (Exception e)
            {
                return new FailureResponseModel()
                {
                    Message = e.Message
                };
            }
        }
    /*        [HttpGet]
            public HttpResponseMessage Get([FromQuery] Document document)
            {
                try
                {
                    var documentService = DocumentService.GetInstance();
                    var documents = documentService.GetDocuments(document);
                    if (documents == null || documents.Count == 0)
                    {
                        return new HttpResponseMessage();
                    }
                    Debug.WriteLine(documents[0].Url);
                    var filestream = new FileStream(documents[0].Url, FileMode.Open);
                    var response = new HttpResponseMessage(HttpStatusCode.OK)
                    {
                        Content = new StreamContent(filestream)
                    };
                    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = documents[0].Title
                    };
                    return response;
                }
                catch
                {
                    return new HttpResponseMessage();
                }
            }*/
        [HttpGet]
        public IActionResult? Get([FromQuery] Document document)
        {
            try
            {
                var documentService = DocumentService.GetInstance();
                var documents = documentService.GetDocuments(document);
                if (documents == null || documents.Count == 0)
                {
                    return NotFound();
                }
                // Debug.WriteLine(documents[0].Url);
                var filestream = new FileStream(documents[0].Url, FileMode.Open);
                return File(filestream,
                            "application/octet-stream",
                            documents[0].Title);
            }
            catch
            {
                return NotFound();
            }
        }
    }
}
