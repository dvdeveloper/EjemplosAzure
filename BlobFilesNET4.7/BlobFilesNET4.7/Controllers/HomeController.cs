using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using BlobFilesNET4._7.Clase;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BlobFilesNET4._7.Controllers
{
    public class HomeController : Controller
    {
        private string connectionString = "DefaultEndpointsProtocol=https;AccountName=dvqimagenes;AccountKey=DsTJLBU2CQOmvY+dArQFWT8CvNk7DmbnJFQnPU6dfKB6qBHlrlbcaQx+8VIevj30uR+F5cLGpRI+uj/iwPJO4Q==;EndpointSuffix=core.windows.net";
        private string containerName = "imagen";

        public ActionResult Index()
        {
            
            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

            List<ArchivoBlob> clase = new List<ArchivoBlob>();
            foreach (BlobItem blob in container.GetBlobs())
            {
                BlobClient blobX = container.GetBlobClient(blob.Name);

                ArchivoBlob archivo = new ArchivoBlob();
                archivo.name = blobX.Name;
                archivo.urlAbsolute = blobX.Uri.AbsoluteUri;
                archivo.BlobContainerName = blobX.BlobContainerName;
                clase.Add(archivo);
            }
            ViewBag.archivos = clase;


            return View();
        }

        public ActionResult Agregar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Agregar(FormCollection collection, HttpPostedFileBase archivo)
        {

            var fileName = Path.GetFileName(archivo.FileName);
            string imageName = Guid.NewGuid().ToString() + "-" + Path.GetExtension(archivo.FileName);

            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            BlobClient blobClient = container.GetBlobClient(imageName);
            Stream uploadFileStream = archivo.InputStream;
            blobClient.Upload(uploadFileStream);

            return RedirectToAction("Index");
        }

        public ActionResult Eliminar(string name)
        {
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);
            var blob = container.GetBlobClient(name);
            blob.DeleteIfExists();
            return RedirectToAction("Index");
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}