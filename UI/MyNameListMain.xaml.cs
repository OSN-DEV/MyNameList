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
using MyNameList.Wording;
using Microsoft.Win32;
using MyLib.File;

namespace MyNameList.UI {
    /// <summary>
    /// MyNameListMain.xaml の相互作用ロジック
    /// </summary>
    public partial class MyNameListMain : Window {
        #region Declaration
        private bool _isChanged = false;
        private string _currentNameListFile = "";
        private readonly NameListOperator _operator = new NameListOperator();
        #endregion

        #region Constructor
        /// <summary>
        /// constructor
        /// </summary>
        public MyNameListMain() {
            InitializeComponent();

            this._isChanged = false;
            this.cNameList.DataContext = this._operator.DataContext;
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
            var item = (MenuItem)sender;
            switch (item.Header.ToString()) {
                case Labels.FileMenuNew:
                    if (this._isChanged && this.ShowSaveConfirmDialog()) {
                        if (!this.RunSaveProcess()) {
                            return;
                        }
                    }
                    this.RunCreateProcess();
                    break;
                case Labels.FileMenuOpen:
                    if (this._isChanged && this.ShowSaveConfirmDialog()) {
                        if (!this.RunSaveProcess()) {
                            return;
                        }
                    }
                    this.RunOpenProcess();
                    break;
                case Labels.FileMenuSave:
                    this.RunSaveProcess();
                    break;
                case Labels.FileMenuSaveAs:
                    this.RunSaveProcess(true);
                    break;
                case Labels.FileMenuExit:
                    this.Close();
                    break;
                default:
                    throw new NotSupportedException(Messages.UnknownMenuItem);
            }
        }

        /// <summary>
        /// 追加ボタンクリックイベント
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Add_Click(object sender, RoutedEventArgs e) {
            this._operator.Add(this.cEnglishName.Text, this.cJapaneseName.Text, this.cNote.Text);
            this.cEnglishName.Focus();
            this.ClearInputArea();
            this.SetStatusChanged();
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
        private void InputArea_TextChanged(object sender, TextChangedEventArgs e) {
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

        /// <summary>
        /// 入力域の初期化
        /// </summary>
        private void ClearInputArea() {
            this.cEnglishName.Text = "";
            this.cJapaneseName.Text = "";
            this.cNote.Text = "";
        }

        /// <summary>
        /// 編集モードに変更
        /// </summary>
        private void SetStatusChanged() {
            this._isChanged = true;
            if (!this.Title.StartsWith(Titles.EditMark)) {
                this.Title = Titles.EditMark + this.Title;
            }
        }

        /// <summary>
        /// 新規ファイルとして保存する
        /// </summary>
        private void SaveAsNewFile() {

        }

        /// <summary>
        /// 上書き保存する
        /// </summary>
        private void SaveOverride() {

        }

        /// <summary>
        /// ファイルの保存プロセスを実行する
        /// </summary>
        /// <param name="isNewFile"></param>
        /// <returns></returns>
        private bool RunSaveProcess() {
            return this.RunSaveProcess(0 == this._currentNameListFile.Length);
        }

        /// <summary>
        /// ファイルの保存プロセスを実行する
        /// </summary>
        /// <param name="isNewFile">true:新規ファイルとしてX保存、false:それ以外</param>
        /// <returns>true:保存成功、false:それ以外</returns>
        private bool RunSaveProcess(bool isNewFile) {
            if (isNewFile) {
                var dialog = new SaveFileDialog();
                dialog.Title = Titles.SaveAs;
                dialog.FileName = Others.NewFileName;
                dialog.Filter = "MNL ファイル|*" + Others.MyNameListExt;
                dialog.FilterIndex = 2;
                if (true == dialog.ShowDialog()) {
                    this._currentNameListFile = dialog.FileName;
                    this._operator.FilePath = this._currentNameListFile;
                }
            }

            if (!this._operator.Save()) {
                var dialog = new CommonErrorDialog(this) {
                    ErrorMessage = Messages.FailToSave,
                    FilePath = this._currentNameListFile
                };
                dialog.ShowDialog();
                return false;
            }

            this._isChanged = false;
            this.Title = _operator.FileName;

            return true;
        }

        /// <summary>
        /// ファイルの保存確認ダイアログを表示する
        /// </summary>
        /// <returns>true:保存を選択、false:それ以外</returns>
        private bool ShowSaveConfirmDialog() {
            return (MessageBoxResult.Yes == MessageBox.Show(Messages.ConfirmSave, Titles.Confirm,
                                                            MessageBoxButton.YesNo, MessageBoxImage.Question));
        }

        /// <summary>
        /// 名称ファイルを作成する。
        /// </summary>
        private void RunCreateProcess() {

        }

        /// <summary>
        /// 名称ファイルを開く。
        /// </summary>
        private void RunOpenProcess() {

        }
        #endregion
    }
}
