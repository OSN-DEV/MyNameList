using System;
using System.Collections.Generic;
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
using MyNameList.Data;
using MyNameList.UI;

namespace MyNameList.Test {
    /// <summary>
    /// AppDataTest.xaml の相互作用ロジック
    /// </summary>
    public partial class Test : Window {
        public Test() {
            InitializeComponent();
        }

        private void Test1(object sender, RoutedEventArgs e) {
            var instance = AppData.GetInstance();
            instance.WindowPosX = 1.0;
            instance.Save();
        }

        private void Test2(object sender, RoutedEventArgs e) {
            var dialog = new CommonErrorDialog(this) {
                DialogTitle = "title",
                ErrorMessage = "message",
                FilePath = "path"
            };
            dialog.ShowDialog();
        }
    }
}
