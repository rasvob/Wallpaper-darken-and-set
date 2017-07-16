using System;
using System.Threading.Tasks;
using MaterialDesignThemes.Wpf;
using WallSetter_v2.ViewModels;

namespace WallSetter_v2.Dialogs
{
    public static class DialogHelper
    {
        public static Task ShowMessage(string title, string message, string host)
        {
            MessageDialog messageDialog = new MessageDialog
            {
                DataContext = new CommonDialogViewModel
                {
                    DialogTitle = title,
                    Message = message
                }
            };

            Task<object> task = DialogHost.Show(messageDialog, host);
            return task;
        }

        public static Task ShowProgressDialog(string title, string message, Action longRunningTask, string host)
        {
            ProgressDialog progressDialog = new ProgressDialog
            {
                DataContext = new CommonDialogViewModel
                {
                    DialogTitle = title,
                    Message = message
                }
            };

            return DialogHost.Show(progressDialog, host, async (sender, args) =>
            {
                await Task.Run(longRunningTask);
                args.Session.Close();
            }, (sender, args) => { });
        }
    }
}