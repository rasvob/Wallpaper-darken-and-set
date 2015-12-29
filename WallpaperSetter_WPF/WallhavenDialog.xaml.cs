using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WallpaperSetter_WPF
{
	/// <summary>
	/// Interaction logic for WallhavenDialog.xaml
	/// </summary>
	/// 

	public delegate void CallbackLink(string link);

	public partial class WallhavenDialog : Window
	{
		public IWallHavenLinkInput LinkCallback { get; set; }
		public CallbackLink LinkCallbackDelegate { get; set; }
		public WallhavenDialog()
		{
			InitializeComponent();
		}

		private void textBoxLink_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
			{
				finishDialogAction();
			}
		}

		private void passLink()
		{
			//LinkCallback.CallbackLink(textBoxLink.Text);
			LinkCallbackDelegate(textBoxLink.Text);
		}

		private void finishDialogAction()
		{
			if(isLinkOk())
			{
				passLink();
				this.Close();
			}
			else
			{
				textBoxLink.Text = "Wrong value";
			}
		}

		private bool isLinkOk()
		{
			string link = textBoxLink.Text;
			if(!link.Contains("wallhaven.cc/wallpaper/"))
			{
				return false;
			}
			else
			{
				string[] parts = link.Split('/');
				string lastPart = parts[parts.Count() - 1];
				if(!checkPart(lastPart))
				{
					return false;
				}
			}
			return true;
		}

		private bool checkPart(string part)
		{
			foreach(char item in part)
			{
				if(!Char.IsDigit(item))
				{
					return false;
				}
			}
			return true;
		}

		private void buttonOk_Click(object sender, RoutedEventArgs e)
		{
			finishDialogAction();
		}
	}
}
