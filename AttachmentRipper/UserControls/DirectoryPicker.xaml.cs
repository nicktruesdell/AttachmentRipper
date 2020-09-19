using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AttachmentRipper.ViewModels;

namespace AttachmentRipper.UserControls
{
    /// <summary>
    /// Interaction logic for DirectoryPicker.xaml
    /// </summary>
    public partial class DirectoryPicker
    {
        public static DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(DirectoryPicker), new FrameworkPropertyMetadata(null, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        
        public string Text
        {
            get => GetValue(TextProperty) as string;
            set => SetValue(TextProperty, value);
        }
        public DirectoryPicker()
        {
            InitializeComponent();
        }

        private void BrowseFolder(object sender, RoutedEventArgs e)
        {
            using (FolderBrowserDialog dlg = new FolderBrowserDialog())
            {
                dlg.Description = "Browse for folder";
                dlg.SelectedPath = Text;
                dlg.ShowNewFolderButton = true;
                DialogResult result = dlg.ShowDialog();
                if (result == System.Windows.Forms.DialogResult.OK)
                {
                    Text = dlg.SelectedPath;
                    BindingExpression be = GetBindingExpression(TextProperty);
                    if (be != null)
                        be.UpdateSource();
                }
            }
        }
    }
}
