using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using WingStudio.Models;

namespace WingStudio
{
    /// <summary>
    /// 辅助类
    /// </summary>
    public static class WebHelper
    {
        /// <summary>
        /// 自定义前台显示信息
        /// </summary>
        /// <param name="title">主题</param>
        /// <param name="content">内容</param>
        /// <param name="func">附加执行语句</param>
        /// <returns>组合成的页面</returns>
         public static string SweetAlert(string title, string content, string func = null)
        {
            if (func == null)
            {
                return "<html><head><link href='/Content/Shared/sweetalert.css' rel='stylesheet'/><script src='/Scripts/sweetalert-dev.js'></script></head><body><script>try{swal({title:'" + title + "', text:'" + content + "'},function(){location.href=document.referrer;});}catch(e){alert('" + title + ":" + content + "');location.href=document.referrer;}</script></body></html>";
            }
            else
            {
                return "<html><head><link href='/Content/Shared/sweetalert.css' rel='stylesheet'/><script src='/Scripts/sweetalert-dev.js'></script></head><body><script>try{swal({title:'" + title + "', text:'" + content + "'},function(){" + func + "});}catch(e){alert('" + title + ":" + content + "');" + func + "}</script></body></html>";
            }
        }

        /// <summary>
        /// 发送邮件
        /// </summary>
        /// <param name="toEmail">目标邮箱</param>
        /// <param name="subjectInfo">邮件标题</param>
        /// <param name="bodyInfo">邮件正文</param>
        /// <returns>true:发送成功，false:发送失败</returns>
        public static bool SendMail(string toEmail, string subjectInfo, string bodyInfo)
        {
            var mailconfig = (EmailConfigurationProvider)ConfigurationManager.GetSection("EmailConfigurationProvider");
            try
            {
                MailAddress from = new MailAddress(mailconfig.Account, mailconfig.Name);
                MailAddress to = new MailAddress(toEmail, toEmail);
                MailMessage message = new MailMessage(from, to);
                message.Subject = subjectInfo;
                message.IsBodyHtml = true;
                message.Body = bodyInfo;
                SmtpClient client = new SmtpClient(mailconfig.Server, mailconfig.Port);
                client.EnableSsl = mailconfig.IsSSL;
                client.Credentials = new NetworkCredential(mailconfig.Account, mailconfig.Password);
                client.Send(message);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 发送http请求
        /// </summary>
        /// <param name="url">目标url</param>
        /// <param name="postParameters">发送数据</param>
        /// <param name="method">访问方式</param>
        /// <returns></returns>
        public static async Task<string> HttpRequest(string url, Dictionary<string, string> postParameters =null, string method = "POST")
        {
            WebRequest request = WebRequest.Create(url);
            request.Method = method;
            request.ContentType = "application/x-www-form-urlencoded";
            if (postParameters != null)
            {
                string postData = "";
                foreach (string key in postParameters.Keys)
                {
                    postData += HttpUtility.UrlEncode(key) + "=" + HttpUtility.UrlEncode(postParameters[key]) + "&";
                }
                byte[] data = Encoding.ASCII.GetBytes(postData);
                request.ContentLength = postData.Length;
                using (Stream requestStream = request.GetRequestStream())
                {
                    await requestStream.WriteAsync(data, 0, postData.Length);
                }
            }
            else
            {
                request.ContentLength = 0;
            }

            using (var streamReader = new StreamReader(request.GetResponse().GetResponseStream()))
            {
                var responseText = streamReader.ReadToEnd();
                return responseText;
            }
           
        }
    }
}