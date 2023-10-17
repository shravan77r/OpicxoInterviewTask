using SSXImport.Business;
using System;
using System.Linq;
using System.Net;
using System.Net.Mail;

namespace SSXImport.WebAPI.Common
{
	/// <summary>
	/// Used to Send and Receive Emails
	/// </summary>
	public class EmailUtility
	{
		/// <summary>
		/// Send Email using given parameters
		/// </summary>
		/// <param name="body"></param>
		/// <param name="subject"></param>
		/// <param name="to"></param>
		/// <param name="organizationId"></param>
		/// <param name="cc"></param>
		/// <param name="bcc"></param>
		/// <param name="attachment"></param>
		/// <returns>Returns flag if Email is sent or not</returns>
		public static bool SendEmail(string body, string subject, string to, int organizationId, string cc = "", string bcc = "", Attachment attachment = null)
		{
			var IsSuccess = false;
			/*
			try
			{
				var settings = new BLSetting().Collection<BLSetting>(
					   new Criteria<BLSetting>()
						   .Add(Expression.Eq("OrganizationId", organizationId))
						   .Add(Expression.Eq("IsDelete", "0"))
						   .Add(Expression.Eq("IsActive", "1"))
						   );
				if (settings != null && settings.Count > 0)
				{
					var email = settings.FirstOrDefault(x => x.Key.Equals(AppConstant.SettingKey.Email_Username.ToString())).Value;
					var password = settings.FirstOrDefault(x => x.Key.Equals(AppConstant.SettingKey.Email_Password.ToString())).Value;
					var host = settings.FirstOrDefault(x => x.Key.Equals(AppConstant.SettingKey.Email_Host.ToString())).Value;
					var port = settings.FirstOrDefault(x => x.Key.Equals(AppConstant.SettingKey.Email_Port.ToString())).Value;
					var enableSsl = settings.FirstOrDefault(x => x.Key.Equals(AppConstant.SettingKey.Email_EnableSSL.ToString())).Value;
					if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(password)
						&& !string.IsNullOrEmpty(host) && !string.IsNullOrEmpty(enableSsl))
					{
						using (MailMessage mailMessage = new MailMessage())
						{
							mailMessage.From = new MailAddress(email);
							mailMessage.To.Add(to);
							mailMessage.Subject = subject;
							mailMessage.Body = body;
							if (attachment != null)
							{
								mailMessage.Attachments.Add(attachment);
							}
							mailMessage.IsBodyHtml = true;
							using (SmtpClient smtpClient = new SmtpClient())
							{
								smtpClient.Host = host;
								smtpClient.EnableSsl = enableSsl != string.Empty ? Convert.ToBoolean(enableSsl) : true;
								smtpClient.UseDefaultCredentials = true;
								smtpClient.Credentials = new NetworkCredential(email, AppConstant.Decrypt(password));
								smtpClient.Port = port != string.Empty ? Convert.ToInt32(port) : 587;
								smtpClient.Send(mailMessage);
								IsSuccess = true;
							}
						}
					}
				}
			}
			catch (Exception ex) { AppConstant.WriteFile(ex.ToString()); }
			*/
			return IsSuccess;
		}
	}
}
