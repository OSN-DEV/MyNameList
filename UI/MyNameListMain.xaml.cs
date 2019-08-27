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
using MyNameList.Component;
using System.Collections.ObjectModel;
using MyNameList.Data;

namespace MyNameList.UI {
    /// <summary>
    /// MyNameListMain.xaml の相互作用ロジック
    /// </summary>
    public partial class MyNameListMain : Window {
        #region Declaration
        private ObservableCollection<NameModel> _nameList = new ObservableCollection<NameModel>();
        #endregion

        #region Constructor
        /// <summary>
        /// constructor
        /// </summary>
        public MyNameListMain() {
            InitializeComponent();

            this.cNameList.DataContext = this._nameList;
            this.Loaded += (sender, e) => {
                this.cEnglishName.Focus();
            };
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

        /// <summary>
        /// 追加ボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e) {
            this._nameList.Add(new NameModel {
                EnglishName = this.cEnglishName.Text,
                JapaneseName = this.cJapaneseName.Text,
                Note = this.cNote.Text
            });
        }

        /// <summary>
        /// 入力エリアキー押下イベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputArea_KeyDown(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                if (Keyboard.Modifiers == ModifierKeys.Control && this.cAdd.IsEnabled) {
                    e.Handled = true;
                    this.Add_Click(null, null);
                } else {
                    var textBox = (TextBoxEx)sender;
                    if (textBox.Name == this.cEnglishName.Name) {
                        e.Handled = true;
                        this.cJapaneseName.Focus();
                    } else if (textBox.Name == this.cJapaneseName.Name) {
                        e.Handled = true;
                        this.cNote.Focus();
                    } else if (textBox.Name == this.cNote.Name && this.cAdd.IsEnabled) {
                        e.Handled = true;
                        this.Add_Click(null, null);
                    }
                }
            }
        }

        /// <summary>
        /// 入力エリアテキスト変更
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InputArea_TextValueChanged(object sender, EventArgs e) {
            this.SetAddButtonEnabled();
        }
        #endregion


        #region Private Method
        /// <summary>
        /// 追加ボタンの使用可否を入力状況に応じて設定
        /// </summary>
        private void SetAddButtonEnabled() {
            this.cAdd.IsEnabled = (0 < this.cEnglishName.Text.Length && 0 < this.cJapaneseName.Text.Length);
        }
        #endregion
    }
}
