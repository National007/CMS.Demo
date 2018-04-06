
using System;
using System.IO;

namespace CMS.MVC.Log
{
    /// <summary>
    /// 通用日志记录器
    /// </summary>
    public static class Logger
    {
        static readonly log4net.ILog loginfo = log4net.LogManager.GetLogger("loginfo");
        static readonly log4net.ILog logerror = log4net.LogManager.GetLogger("logerror");
        static readonly log4net.ILog logmonitor = log4net.LogManager.GetLogger("logmonitor");

        /// <summary>
        /// 
        /// </summary>
        static Logger()
        {
            FileInfo log4netFile =
                new FileInfo(string.Format("{0}/Config/log4net.config", AppDomain.CurrentDomain.BaseDirectory));
            log4net.Config.XmlConfigurator.ConfigureAndWatch(log4netFile);
        }

        /// <summary>  
        /// 记录错误日志  
        /// </summary>  
        /// <param name="ErrorMsg"></param>  
        /// <param name="ex"></param>  
        public static void Error(string ErrorMsg, Exception ex = null)
        {
            if (ex != null)
            {
                logerror.Error(ErrorMsg, ex);
            }
            else
            {
                logerror.Error(ErrorMsg);
            }
        }

        public static void Info(string Msg)
        {
            loginfo.Info(Msg);
        }

        public static void Monitor(string Msg)
        {
            logmonitor.Info(Msg);
        }

    }
}
