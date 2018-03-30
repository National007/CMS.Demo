
using System.IO;
using System.Web;

namespace Application.PictureServices
{
    public interface IPictureService
    {
        string SavePictureByStream(Stream file,string extensions, string savePath);

        string SavePictureByPostFile(HttpPostedFileBase file,string savePath);

        void SaveFileByByte(string fileName, byte[] fileContent,string path);

        void DeleteFile(string deletePath);
    }
}
