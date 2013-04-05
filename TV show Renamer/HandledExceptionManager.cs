/*****************************************************************************************/
/* ***************************************************************************************
 * HandledExceptionManager.cs
 * ***************************************************************************************/
/*****************************************************************************************/
//'--
//'--
//'-- Jeff Atwood
//'-- http://www.codinghorror.com
//'--

using System;
using System.Diagnostics;
using System.Drawing;
using System.Threading;
using System.Windows.Forms;

//'--
//'-- Generic HANDLED error handling class
//'--
//'-- It's like MessageBox, but specific to handled exceptions, and supports email notifications

namespace TV_Show_Renamer
{
	public class HandledExceptionManager
	{
		private static bool _blnHaveException = false;
		private static bool _blnEmailError = true;
		private static string _strEmailBody;
		private static string _strExceptionType;

		private const string _strDefaultMore = "No further information is available. If the problem persists, contact (contact).";
		public static bool EmailError
		{
			get { return _blnEmailError; }
			set { _blnEmailError = value; }
		}

		public enum UserErrorDefaultButton
		{
			Default = 0,
			Button1 = 1,
			Button2 = 2,
			Button3 = 3
		}

		//--
		//-- replace generic constants in strings with specific values
		//--
		private static string ReplaceStringVals(string strOutput)
		{
			string strTemp = null;
			if (strOutput == null)
			{
				strTemp = "";
			}
			else
			{
				strTemp = strOutput;
			}
			strTemp = strTemp.Replace("(app)", AppSettings.AppProduct);
			strTemp = strTemp.Replace("(contact)", AppSettings.GetString("UnhandledExceptionManager/ContactInfo"));
			return strTemp;
		}

		//--
		//-- make sure "More" text is populated with something useful
		//--
		private static string GetDefaultMore(string strMoreDetails)
		{
			if (string.IsNullOrEmpty(strMoreDetails))
			{
				System.Text.StringBuilder objStringBuilder = new System.Text.StringBuilder();
				var _with1 = objStringBuilder;
				_with1.Append(_strDefaultMore);
				_with1.Append(Environment.NewLine);
				_with1.Append(Environment.NewLine);
				_with1.Append("Basic technical information follows: " + Environment.NewLine);
				_with1.Append("---" + Environment.NewLine);
				_with1.Append(UnhandledExceptionManager.SysInfoToString(true));
				return objStringBuilder.ToString();
			}
			else
			{
				return strMoreDetails;
			}
		}

		//--
		//-- converts exception to a formatted "more" string
		//--
		private static string ExceptionToMore(System.Exception objException)
		{
			System.Text.StringBuilder sb = new System.Text.StringBuilder();
			var _with2 = sb;
			if (_blnEmailError)
			{
				_with2.Append("Information about this problem was automatically mailed to ");
				_with2.Append(AppSettings.GetString("UnhandledExceptionManager/EmailTo"));
				_with2.Append(Environment.NewLine + Environment.NewLine);
			}
			_with2.Append("Detailed technical information follows: " + Environment.NewLine);
			_with2.Append("---" + Environment.NewLine);
			string x = UnhandledExceptionManager.ExceptionToString(objException);
			_with2.Append(x);
			return sb.ToString();
		}

		//--
		//-- perform our string replacements for (app) and (contact), etc etc
		//-- also make sure More has default values if it is blank.
		//--

		private static void ProcessStrings(ref string strWhatHappened, ref string strHowUserAffected, ref string strWhatUserCanDo, ref string strMoreDetails)
		{
			strWhatHappened = ReplaceStringVals(strWhatHappened);
			strHowUserAffected = ReplaceStringVals(strHowUserAffected);
			strWhatUserCanDo = ReplaceStringVals(strWhatUserCanDo);
			strMoreDetails = ReplaceStringVals(GetDefaultMore(strMoreDetails));
		}

		//--
		//-- simplest possible error dialog
		//--
		public static DialogResult ShowDialog(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo)
		{
			_blnHaveException = false;
			return ShowDialogInternal(strWhatHappened, strHowUserAffected, strWhatUserCanDo, "", MessageBoxButtons.OK, MessageBoxIcon.Warning, UserErrorDefaultButton.Default);
		}

		//--
		//-- advanced error dialog with Exception object
		//--
		public static DialogResult ShowDialog(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo, System.Exception objException, MessageBoxButtons Buttons = MessageBoxButtons.OK, MessageBoxIcon Icon = MessageBoxIcon.Warning, UserErrorDefaultButton DefaultButton = UserErrorDefaultButton.Default)
		{
			_blnHaveException = true;
			_strExceptionType = objException.GetType().FullName;
			return ShowDialogInternal(strWhatHappened, strHowUserAffected, strWhatUserCanDo, ExceptionToMore(objException), Buttons, Icon, DefaultButton);
		}

		//--
		//-- advanced error dialog with More string
		//-- leave "more" string blank to get the default
		//--
		public static DialogResult ShowDialog(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo, string strMoreDetails, MessageBoxButtons Buttons = MessageBoxButtons.OK, MessageBoxIcon Icon = MessageBoxIcon.Warning, UserErrorDefaultButton DefaultButton = UserErrorDefaultButton.Default)
		{
			_blnHaveException = false;
			return ShowDialogInternal(strWhatHappened, strHowUserAffected, strWhatUserCanDo, strMoreDetails, Buttons, Icon, DefaultButton);
		}

		//--
		//-- internal method to show error dialog
		//--
		private static DialogResult ShowDialogInternal(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo, string strMoreDetails, MessageBoxButtons Buttons, MessageBoxIcon Icon, UserErrorDefaultButton DefaultButton)
		{
			//-- set default values, etc
			ProcessStrings(ref strWhatHappened, ref strHowUserAffected, ref strWhatUserCanDo, ref strMoreDetails);

			ExceptionDialog objForm = new ExceptionDialog();
			var _with3 = objForm;
			_with3.Text = ReplaceStringVals(objForm.Text);
			_with3.ErrorBox.Text = strWhatHappened;
			_with3.ScopeBox.Text = strHowUserAffected;
			_with3.ActionBox.Text = strWhatUserCanDo;
			_with3.txtMore.Text = strMoreDetails;

			//-- determine what button text, visibility, and defaults are
			var _with4 = objForm;
			switch (Buttons)
			{
				case MessageBoxButtons.AbortRetryIgnore:
					_with4.btn1.Text = "&Abort";
					_with4.btn2.Text = "&Retry";
					_with4.btn3.Text = "&Ignore";
					_with4.AcceptButton = objForm.btn2;
					_with4.CancelButton = objForm.btn3;
					break;
				case MessageBoxButtons.OK:
					_with4.btn3.Text = "OK";
					_with4.btn2.Visible = false;
					_with4.btn1.Visible = false;
					_with4.AcceptButton = objForm.btn3;
					break;
				case MessageBoxButtons.OKCancel:
					_with4.btn3.Text = "Cancel";
					_with4.btn2.Text = "OK";
					_with4.btn1.Visible = false;
					_with4.AcceptButton = objForm.btn2;
					_with4.CancelButton = objForm.btn3;
					break;
				case MessageBoxButtons.RetryCancel:
					_with4.btn3.Text = "Cancel";
					_with4.btn2.Text = "&Retry";
					_with4.btn1.Visible = false;
					_with4.AcceptButton = objForm.btn2;
					_with4.CancelButton = objForm.btn3;
					break;
				case MessageBoxButtons.YesNo:
					_with4.btn3.Text = "&No";
					_with4.btn2.Text = "&Yes";
					_with4.btn1.Visible = false;
					break;
				case MessageBoxButtons.YesNoCancel:
					_with4.btn3.Text = "Cancel";
					_with4.btn2.Text = "&No";
					_with4.btn1.Text = "&Yes";
					_with4.CancelButton = objForm.btn3;
					break;
			}

			////-- set the proper dialog icon
			//switch (Icon) {
			//	case MessageBoxIcon.Error:
			//		objForm.PictureBox1.Image = System.Drawing.SystemIcons.Error.ToBitmap();
			//		break;
			//	case MessageBoxIcon.Stop:
			//		objForm.PictureBox1.Image = System.Drawing.SystemIcons.Error.ToBitmap();
			//		break;
			//	case MessageBoxIcon.Exclamation:
			//		objForm.PictureBox1.Image = System.Drawing.SystemIcons.Exclamation.ToBitmap();
			//		break;
			//	case MessageBoxIcon.Information:
			//		objForm.PictureBox1.Image = System.Drawing.SystemIcons.Information.ToBitmap();
			//		break;
			//	case MessageBoxIcon.Question:
			//		objForm.PictureBox1.Image = System.Drawing.SystemIcons.Question.ToBitmap();
			//		break;
			//	default:
			//		objForm.PictureBox1.Image = System.Drawing.SystemIcons.Error.ToBitmap();
			//		break;
			//}

			//'-- set the proper dialog icon
			MessageBoxIcon PictureBox1 = Icon;
			if (PictureBox1 == MessageBoxIcon.Hand)
			{
				objForm.PictureBox1.Image = SystemIcons.Error.ToBitmap();
			}
			else if (PictureBox1 == MessageBoxIcon.Hand)
			{
				objForm.PictureBox1.Image = SystemIcons.Error.ToBitmap();
			}
			else if (PictureBox1 == MessageBoxIcon.Exclamation)
			{
				objForm.PictureBox1.Image = SystemIcons.Exclamation.ToBitmap();
			}
			else if (PictureBox1 == MessageBoxIcon.Asterisk)
			{
				objForm.PictureBox1.Image = SystemIcons.Information.ToBitmap();
			}
			else if (PictureBox1 == MessageBoxIcon.Question)
			{
				objForm.PictureBox1.Image = SystemIcons.Question.ToBitmap();
			}
			else
			{
				objForm.PictureBox1.Image = SystemIcons.Error.ToBitmap();
			}

			//-- override the default button
			switch (DefaultButton)
			{
				case UserErrorDefaultButton.Button1:
					objForm.AcceptButton = objForm.btn1;
					objForm.btn1.TabIndex = 0;
					break;
				case UserErrorDefaultButton.Button2:
					objForm.AcceptButton = objForm.btn2;
					objForm.btn2.TabIndex = 0;
					break;
				case UserErrorDefaultButton.Button3:
					objForm.AcceptButton = objForm.btn3;
					objForm.btn3.TabIndex = 0;
					break;
			}

			if (_blnEmailError)
			{
				SendNotificationEmail(strWhatHappened, strHowUserAffected, strWhatUserCanDo, strMoreDetails);
			}

			//-- show the user our error dialog
			return objForm.ShowDialog();
		}

		//--
		//-- this is the code that executes in the spawned thread
		//--
		private static void ThreadHandler()
		{
			SimpleMail.SMTPClient smtp = new SimpleMail.SMTPClient();
			SimpleMail.SMTPMailMessage mail = new SimpleMail.SMTPMailMessage();
			var _with5 = mail;
			_with5.To = AppSettings.GetString("UnhandledExceptionManager/EmailTo");
			if (_blnHaveException)
			{
				_with5.Subject = "Handled Exception notification - " + _strExceptionType;
			}
			else
			{
				_with5.Subject = "HandledExceptionManager notification";
			}
			_with5.Body = _strEmailBody;
			//-- try to send email, but we don't care if it succeeds (for now)
			try
			{
				smtp.SendMail(mail);
			}
			catch (Exception e)
			{
				Debug.WriteLine("** SMTP email failed to send!");
				Debug.WriteLine("** " + e.Message);
			}
		}

		//--
		//-- send notification about this error via e-mail
		//--

		private static void SendNotificationEmail(string strWhatHappened, string strHowUserAffected, string strWhatUserCanDo, string strMoreDetails)
		{
			//-- ignore debug exceptions (eg, development testing)?
			if (UnhandledExceptionManager.IgnoreDebugErrors)
			{
				if (AppSettings.DebugMode)
					return;
			}

			System.Text.StringBuilder objStringBuilder = new System.Text.StringBuilder();
			var _with6 = objStringBuilder;
			_with6.Append("What happened:");
			_with6.Append(Environment.NewLine);
			_with6.Append(strWhatHappened);
			_with6.Append(Environment.NewLine);
			_with6.Append(Environment.NewLine);
			_with6.Append("How this will affect the user:");
			_with6.Append(Environment.NewLine);
			_with6.Append(strHowUserAffected);
			_with6.Append(Environment.NewLine);
			_with6.Append(Environment.NewLine);
			_with6.Append("What the user can do about it:");
			_with6.Append(Environment.NewLine);
			_with6.Append(strWhatUserCanDo);
			_with6.Append(Environment.NewLine);
			_with6.Append(Environment.NewLine);
			_with6.Append("More information:");
			_with6.Append(Environment.NewLine);
			_with6.Append(strMoreDetails);
			_with6.Append(Environment.NewLine);
			_with6.Append(Environment.NewLine);
			SendEmail(objStringBuilder.ToString());
		}

		private static void SendEmail(string strEmailBody)
		{
			_strEmailBody = strEmailBody;
			//-- spawn off the email send attempt as a thread for improved throughput
			Thread objThread = new Thread(new ThreadStart(ThreadHandler));
			objThread.Name = "HandledExceptionEmail";
			objThread.Start();
		}
	}
}