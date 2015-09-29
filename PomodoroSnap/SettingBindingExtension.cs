using System.Windows.Data;

namespace PomodoSnap
{
    class SettingBindingExtension : Binding
    {
        public SettingBindingExtension()
        {
            Initialize();
        }

        public SettingBindingExtension(string path)
            : base(path)
        {
            Initialize();
        }

        private void Initialize()
        {
            this.Source = PomodoSnap.Properties.Settings.Default;
            this.Mode = BindingMode.TwoWay;
        }
    }
}
