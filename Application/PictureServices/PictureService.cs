
using System;
using System.IO;
using System.Web;

namespace Application.PictureServices
{
    public class PictureService :  IPictureService
    {
       
        /// <summary>
        /// 保存图片
        /// </summary>
        /// <param name="file"></param>
        /// <param name="savePath"></param>
        /// <returns></returns>
        public  virtual string SavePictureByPostFile(HttpPostedFileBase file, string savePath)
        {
            string fileName = Path.GetFileName(file.FileName);
            string extend = Path.GetExtension(fileName);
            //string saveName = string.Format("{0}{1}",Guid.NewGuid().ToString(),extend);

            Stream s = file.InputStream;

            return SavePictureByStream(s, extend, savePath);

            //return saveName;
        }
        /// <summary>
        /// 保存文件
        /// </summary>
        /// <param name="s"></param>
        /// <param name="extensions"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public virtual string SavePictureByStream(Stream s, string extensions, string path)
        {
            string saveName = string.Format("{0}{1}", Guid.NewGuid().ToString(), extensions);
            byte[] fileBytes = new byte[s.Length];
            s.Read(fileBytes, 0, fileBytes.Length);


            SaveFileByByte(saveName, fileBytes, path);
            return saveName;
        }

        /// <summary>
        /// 删除文件 
        /// </summary>
        /// <param name="fullName"></param>
        /// <param name="path"></param>
        public virtual void DeleteFile(string deletePath)
        {
            System.IO.File.Delete(deletePath);
        }

        public virtual void SaveFileByByte(string fileName, byte[] fileContent, string path)
        {
            string savePath = Path.Combine(path, fileName);

            System.IO.File.WriteAllBytes(savePath, fileContent);
        }


    }
}
