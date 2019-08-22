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

namespace MyNameList.Test {
    /// <summary>
    /// AppDataTest.xaml の相互作用ロジック
    /// </summary>
    public partial class AppDataTest : Window {
        public AppDataTest() {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e) {
            var instance = AppData.GetInstance();
            instance.WindowPosX = 1.0;
            instance.Save();
        }
    }
}
