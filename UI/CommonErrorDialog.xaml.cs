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
using MyNameList.Util;
using MyNameList.Data;
using MyNameList.Wording;

namespace MyNameList.UI {
    /// <summary>
    /// 汎用エラーダイアログ
    /// </summary>
    public partial class CommonErrorDialog : Window {

        #region Public Property
        /// <summary>
        /// ダイアログのタイトル。省略時は"エラー"を設定。
        /// </summary>
        public string DialogTitle { set; get; } = Titles.Error;
        public string ErrorMessage { set; get; }
        public string FilePath { set; get; }
        #endregion

        #region Constructor
        /// <summary>
        /// Constructor
        /// </summary>
        public CommonErrorDialog(Window owner) {
            this.Owner = owner;
            InitializeComponent();

            this.Loaded += delegate {
                this.DataContext = new CommonErrorDialogModel() {
                    DialogTitle = this.DialogTitle,
                    ErrorMessage = this.ErrorMessage,
                    FilePath = this.FilePath
                };
            };
        }
        #endregion

        #region Event
        /// <summary>
        /// OK Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OK_Click(object sender, RoutedEventArgs e) {
            this.DialogResult = true;
        }
        #endregion
    }
}
