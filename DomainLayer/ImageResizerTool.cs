using System.Web;
using ImageResizer;

namespace DomainLayer
{
    public class ImageResizerTool
    {
        public void ResizeImageData(HttpPostedFileBase pictureFile, string fileName, string storageRoot)
        {
            var i = new ImageJob(pictureFile, storageRoot + fileName, new ResizeSettings(
           "format=jpg;mode=max"));
            i.Build();
        } 
    }
}