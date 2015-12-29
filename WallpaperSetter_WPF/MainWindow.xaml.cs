using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WalllpaperSetter_WPF;
using System.Diagnostics;

namespace WallpaperSetter_WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window, IWallHavenLinkInput
	{
		private string pathToOpenedImg;
		private PreviewCreator previewCreator;
		private WallhavenDownloader wallhavenDownloader;
		private WallSetter.Style styleOfWall;
		private int opacity;

		public MainWindow()
		{
			InitializeComponent();
			opacity = 0;
			styleOfWall = WallSetter.Style.Tiled;
			previewCreator = new PreviewCreator();
			wallhavenDownloader = new WallhavenDownloader();
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			await Task.Run(() =>
			 {
				 WallSetter setter = new WallSetter(pathToOpenedImg);
				 setter.StyleOfWallpaper = styleOfWall;
				 setter.SetDesktopWallpaper(opacity);
			 });
			
		}

		private void MenuItem_Click(object sender, RoutedEventArgs e)
		{
			var fileDialog = new System.Windows.Forms.OpenFileDialog();
			fileDialog.Filter = "Image files (*.jpg, *.jpeg, *.bmp, *.png) | *.jpg; *.jpeg; *.bmp; *.png";
			var result = fileDialog.ShowDialog();
			switch(result)
			{
				case System.Windows.Forms.DialogResult.None:
					break;
				case System.Windows.Forms.DialogResult.OK:
					FileInfo fi = new FileInfo(fileDialog.FileName);
					pathToOpenedImg = fi.FullName;
					setupPreviewCreator();
					image.Source = previewCreator.CreatePreview();
					break;
				case System.Windows.Forms.DialogResult.Cancel:
					break;
				case System.Windows.Forms.DialogResult.Abort:
					break;
				case System.Windows.Forms.DialogResult.Retry:
					break;
				case System.Windows.Forms.DialogResult.Ignore:
					break;
				case System.Windows.Forms.DialogResult.Yes:
					break;
				case System.Windows.Forms.DialogResult.No:
					break;
				default:
					break;
			}
		}


		private void darkenImage()
		{
			if(previewCreator.IsReady())
			{
				previewCreator.Width = (int)image.ActualWidth;
				previewCreator.Height = (int)image.ActualHeight;
				opacity = (int)slider.Value;
				image.Source = previewCreator.DarkenPreview(opacity);
			}
		}

		private void setupPreviewCreator()
		{
			previewCreator.PathToImage = pathToOpenedImg;
			previewCreator.Width = (int)image.ActualWidth;
			previewCreator.Height = (int)image.ActualHeight;
			slider.Value = slider.Minimum;
		}

		private void slider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			darkenImage();
		}
		
		private void RadioButton1_Checked(object sender, RoutedEventArgs e)
		{

			styleOfWall = WallSetter.Style.Tiled;
		}

		private void RadioButton2_Checked(object sender, RoutedEventArgs e)
		{
			styleOfWall = WallSetter.Style.Centered;
		}

		private void RadioButton3_Checked(object sender, RoutedEventArgs e)
		{
			styleOfWall = WallSetter.Style.Stretched;
		}

		private void menuItemWallhaven_Click(object sender, RoutedEventArgs e)
		{
			Trace.WriteLine("Menu item wallhaven clicked");
			WallhavenDialog dialog = new WallhavenDialog();
			dialog.LinkCallback = this;
			dialog.ShowDialog();
		}

		public void CallbackLink(string link)
		{
			Trace.WriteLine(link);
			wallhavenDownloader.WallhavenLink = link;
		}
	}
}
