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
using System.Windows.Forms;
using System.Threading;

namespace WallpaperSetter_WPF
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
	{
		private string _pathToOpenedImg;
		private PreviewCreator _previewCreator;
		private WallhavenDownloader _wallhavenDownloader;
		private WallSetter.Style _styleOfWall;
		private int opacity;
		private MemoryStream _previewStream;

		public MainWindow()
		{
			InitializeComponent();
			opacity = 0;
			_styleOfWall = WallSetter.Style.Tiled;
			_previewCreator = new PreviewCreator();
			_wallhavenDownloader = new WallhavenDownloader();
			_previewStream = new MemoryStream();

			_wallhavenDownloader.CallbackDownloadComplete = (filename) => {
				UpdateInfoAboutOpenedImage(filename);
			};

			_previewCreator.OutputStream = _previewStream;
		}

		private void testCallback(BitmapImage bi)
		{
			image.Source = null;
		}

		private async void button_Click(object sender, RoutedEventArgs e)
		{
			await Task.Run(() =>
			 {
				 WallSetter setter = new WallSetter(_pathToOpenedImg);
				 setter.StyleOfWallpaper = _styleOfWall;
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
					UpdateInfoAboutOpenedImage(fileDialog.FileName);
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

		private void UpdateInfoAboutOpenedImage(string filename)
		{
			FileInfo fi = new FileInfo(filename);
			_pathToOpenedImg = fi.FullName;
			setupPreviewCreator();
			image.Source = _previewCreator.CreatePreview();
		}

		private void darkenImage()
		{
			if(_previewCreator.IsReady())
			{
				_previewCreator.Width = (int)image.ActualWidth;
				_previewCreator.Height = (int)image.ActualHeight;
				opacity = (int)slider.Value;
				Task.Factory.StartNew(() => _previewCreator.DarkenPreviewNoRes(opacity)).ContinueWith(t => image.Source = _previewCreator.MemoryStreamToBitmapImage(_previewStream), TaskScheduler.FromCurrentSynchronizationContext());
				//image.Source = _previewCreator.DarkenPreview(opacity);
			}
		}

		private void setupPreviewCreator()
		{
			_previewCreator.PathToImage = _pathToOpenedImg;
			_previewCreator.Width = (int)image.ActualWidth;
			_previewCreator.Height = (int)image.ActualHeight;
			slider.Value = slider.Minimum;
		}

		private void slider_PreviewMouseUp(object sender, MouseButtonEventArgs e)
		{
			darkenImage();
		}
		
		private void RadioButton1_Checked(object sender, RoutedEventArgs e)
		{

			_styleOfWall = WallSetter.Style.Tiled;
		}

		private void RadioButton2_Checked(object sender, RoutedEventArgs e)
		{
			_styleOfWall = WallSetter.Style.Centered;
		}

		private void RadioButton3_Checked(object sender, RoutedEventArgs e)
		{
			_styleOfWall = WallSetter.Style.Stretched;
		}

		private void menuItemWallhaven_Click(object sender, RoutedEventArgs e)
		{
			WallhavenDialog dialog = new WallhavenDialog();
			dialog.LinkCallbackDelegate = (t) =>
			{
				Trace.WriteLine(t);
				_wallhavenDownloader.WallhavenLink = t;
				_wallhavenDownloader.DownloadImageFromWallhaven();
			};
			dialog.ShowDialog();
		}
	}
}
