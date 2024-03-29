﻿/******************************************
  2022 Trimester 3 INFT6900 Final Project
  Team   : Four Square
  Author : Weiran Wang
  Date   : 01/11/2022
******************************************/

using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Diagnostics;
using System.Net.Mail;
using System.Security.Policy;
using WebAPI.DAO;
using WebAPI.Entity;

namespace WebAPI.Service
{
    public class DocumentService
    {
        private DocumentService()
        {
            UploadRootPath = SysConfigModel
                            .Configuration
                            .GetConnectionString("UploadRootPath");
        }
        private static DocumentService Instance = new DocumentService();
        public static DocumentService GetInstance()
        {
            Instance ??= new DocumentService();
            return Instance;
        }
        private readonly string? UploadRootPath;

        public void AddDocuments(List<IFormFile> files,
                                 long applicationID,
                                 string type)
        {
            // TODO: authorize the application
            var uploadPath = UploadRootPath + "/"
                           + applicationID.ToString() + "/";
            if (!Directory.Exists(uploadPath))
                Directory.CreateDirectory(uploadPath);
            var documentDAO = new DocumentDAO();

            var documents = new List<Document>();
            files.ForEach(file =>
            {
                var url = uploadPath + file.FileName;
                if (!File.Exists(url)
                    && documentDAO.Query(new Document()
                                        {
                                            Url = url
                                        }).Count != 0)
                {
                    documents.Add(new Document()
                    {
                        ApplicationID = applicationID,
                        Type = type,
                        Title = file.FileName,
                        Url = url
                    });
                }
                Debug.WriteLine(url);
                // save file
                var stream = file.OpenReadStream();
                //convert stream to byte[]
                byte[] bytes = new byte[stream.Length];
                stream.Read(bytes, 0, bytes.Length);
                // set stream beginning
                stream.Seek(0, SeekOrigin.Begin);
                // write byte[] to file
                var fs = new FileStream(url, FileMode.Create);
                var bw = new BinaryWriter(fs);
                bw.Write(bytes);
                bw.Close();
                fs.Close(); // TODO: use only one instance

                documents.Add(new Document()
                {
                    ApplicationID = applicationID,
                    Type = type,
                    Title = file.FileName,
                    Url = url
                });
            });

            documentDAO.Insert(documents);
        }

        public List<Document> GetDocuments(Document document)
        {
            return new DocumentDAO().Query(document);
        }

        public byte[] GetFile(Document document)
        {
            var fileStream = new FileStream(document.Url,
                                            FileMode.Open,
                                            FileAccess.Read);
            byte[] bytes = new byte[fileStream.Length];
            fileStream.Read(bytes, 0, bytes.Length);
            return bytes;
        }

        public void DeleteDocument(int documentID)
        {
            new DocumentDAO().Delete(new List<Document>()
            {
                new Document()
                {
                    DocumentID = documentID
                }
            });
        }
    }
}
