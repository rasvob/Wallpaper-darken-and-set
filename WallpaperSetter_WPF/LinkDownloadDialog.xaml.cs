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
	/// Interaction logic for LinkDownloadDialog.xaml
	/// </summary>
	public partial class LinkDownloadDialog : Window
	{
		private string _link;

		public event EventHandler<string> LinkSet;

		public LinkDownloadDialog()
		{
			InitializeComponent();
		}

		private void buttonOk_Click(object sender, RoutedEventArgs e)
		{
			FinishDialog();
		}

		private void textBoxLink_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.Key == Key.Enter)
			{
				FinishDialog();
			}
		}

		private void FinishDialog()
		{
			if(IsLinkOk())
			{
				_link = textBoxLink.Text;
				OnLinkSet();
				this.Close();
			}
			else
			{
				textBoxLink.Text = "Wrong link";
			}
		}

		private bool IsLinkOk()
		{
			string link = textBoxLink.Text;

			try
			{
				int indexOfDot = link.LastIndexOf(".");
				string ext = link.Substring(indexOfDot);
				string[] allowedExt = { ".jpg", ".jpeg", ".png", ".bmp" };

				if(allowedExt.Contains(ext))
				{
					return true;
				}
			}
			catch(ArgumentOutOfRangeException ex)
			{
				return false;
			}

			return false;
		}

		protected virtual void OnLinkSet()
		{
			if(LinkSet != null)
			{
				LinkSet(this, _link);
			}
		}
	}
}
