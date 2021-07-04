using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AzureActiveDirectoryEJemplo.Controllers
{
    [System.Web.Mvc.Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //https://docs.microsoft.com/en-us/azure/active-directory/develop/tutorial-v2-asp-webapp
            var userClaims = User.Identity as System.Security.Claims.ClaimsIdentity;
            //You get the user's first and last name below:
            ViewBag.Name = userClaims?.FindFirst("name")?.Value;
            // The 'preferred_username' claim can be used for showing the username
            ViewBag.Username = userClaims?.FindFirst("preferred_username")?.Value;
            // The subject/ NameIdentifier claim can be used to uniquely identify the user across the web
            ViewBag.Subject = userClaims?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            // TenantId is the unique Tenant Id - which represents an organization in Azure AD
            ViewBag.TenantId = userClaims?.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid")?.Value;



















            //Cuenta Storage blob (imagenes u otros)
            //https://github.com/Azure/azure-sdk-for-net/blob/master/sdk/storage/Azure.Storage.Blobs/README.md

            // Get a connection string to our Azure Storage account.
            string connectionString = "DefaultEndpointsProtocol=https;AccountName=dvqimagenes;AccountKey=DsTJLBU2CQOmvY+dArQFWT8CvNk7DmbnJFQnPU6dfKB6qBHlrlbcaQx+8VIevj30uR+F5cLGpRI+uj/iwPJO4Q==;EndpointSuffix=core.windows.net";
            string containerName = "imagen";
            // Get a reference to a container named "sample-container" and then create it
            BlobContainerClient container = new BlobContainerClient(connectionString, containerName);

            //ViewBag.fotos = container.GetBlobs();

            List<string> src = new List<string>();
            foreach (BlobItem blob in container.GetBlobs())
            {
                string url = container.GetBlobClient(blob.Name).Uri.AbsoluteUri;
                src.Add(url);
            }
            ViewBag.fotos = src;

            return View();
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