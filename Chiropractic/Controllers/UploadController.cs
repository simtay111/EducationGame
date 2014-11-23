using System.IO;
using System.Web.Mvc;
using DomainLayer;
using EducationGame.Controllers.CustomResults;

namespace EducationGame.Controllers
{
    public class UploadController : Controller
    {
        private string StorageRoot
        {
            get { return Path.Combine(System.Web.HttpContext.Current.Server.MapPath("~/Files/AcctLogos/")); }
        }

        [HttpPost]
        [Authorize]
        public JsonDotNetResult Image()
        {
            string webUseFullPath = string.Empty;
            for (int i = 0; i < HttpContext.Request.Files.Count; i++)
            {
                var file = HttpContext.Request.Files[i];

                var myTool = new ImageResizerTool();

                var fileName = (int)Session[SessionConstants.AcctInfoId] + Constants.ImageExt;

                myTool.ResizeImageData(file, fileName, StorageRoot);

                webUseFullPath = Constants.ImageBaseUrl + fileName;
                file.SaveAs(StorageRoot + fileName);
            }

            return new JsonDotNetResult { Data = new { filename = webUseFullPath } };
        }
        [HttpPost]
        [Authorize]
        public void Delete()
        {
            var fileName = (int)Session[SessionConstants.AcctInfoId] + Constants.ImageExt;
            System.IO.File.Delete(StorageRoot + fileName);
        }
    }
}