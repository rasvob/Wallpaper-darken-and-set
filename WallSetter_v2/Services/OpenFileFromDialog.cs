using Microsoft.Win32;

namespace WallSetter_v2.Services
{
    public class OpenFileFromDialog: IOpenFileService
    {
        public string OpenFile()
        {
            var fileDialog = new OpenFileDialog
            {
                Filter = "Image files (*.jpg, *.jpeg, *.bmp, *.png) | *.jpg; *.jpeg; *.bmp; *.png"
            };
            var result = fileDialog.ShowDialog();
            if (result.HasValue && result.Value)
            {
                return fileDialog.FileName;
            }
            return null;
        }
    }
}