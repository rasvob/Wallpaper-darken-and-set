using Microsoft.Win32;

namespace WallSetter_v2.Services
{
    public class SaveFileFromDialog: ISaveFileService
    {
        public string SaveFile()
        {
            var fileDialog = new SaveFileDialog
            {
                Filter = "Image files (*.png) | *.png",
                AddExtension = true,
                DefaultExt = "png",
                CheckPathExists = true,
                Title = "wallpaper"
            };
            var result = fileDialog.ShowDialog();
            if (!result.HasValue || !result.Value) return null;
            string file = fileDialog.FileName;
            return file;
        }
    }
}