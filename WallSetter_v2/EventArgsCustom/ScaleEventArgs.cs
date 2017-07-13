using System;

namespace WallSetter_v2.EventArgsCustom
{
    public class ScaleEventArgs: System.EventArgs
    {
        public double OldScale { get; set; }
        public double OldHorizontalOffset { get; set; }
        public double OldVerticalOffset { get; set; }
        public double OldViewportHeight { get; set; }
        public double OldViewportWidth { get; set; }
    }
}