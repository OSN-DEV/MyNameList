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

namespace MyNameList.UI {
    /// <summary>
    /// MyNameListMain.xaml の相互作用ロジック
    /// </summary>
    public partial class MyNameListMain : Window {
        #region Constructor
        /// <summary>
        /// constructor
        /// </summary>
        public MyNameListMain() {
            InitializeComponent();
        }
        #endregion

        #region Event
        /// <summary>
        /// ファイルメニューのメニューアイテムクリック時のイベント
        /// </summary>
        /// <param name="sender">メニューアイテム</param>
        /// <param name="e"></param>
        private void FileMenuItem_Click(object sender, RoutedEventArgs e) {

        }
        #endregion
    }
}
