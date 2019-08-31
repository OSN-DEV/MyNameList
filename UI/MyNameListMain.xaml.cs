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
        private AppData _appData = AppData.GetInstance();
        private const int MaxRecentFileCount = 5;
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
            this.CreateRecentFilesMenu();
            if (0 <= this._appData.WindowPosX && (this._appData.WindowPosX + this._appData.WindowSizeW) < SystemParameters.VirtualScreenWidth) {
                this.Left = this._appData.WindowPosX;
            }
            if (0 <= this._appData.WindowPosY && (this._appData.WindowPosY + this._appData.WindowSizeH) < SystemParameters.VirtualScreenHeight) {
                this.Top = this._appData.WindowPosY;
            }
            if (0 < this._appData.WindowSizeW && this._appData.WindowSizeW <= SystemParameters.WorkArea.Width) {
                this.Width = this._appData.WindowSizeW;
            }
            if (0 < this._appData.WindowSizeH && this._appData.WindowSizeH <= SystemParameters.WorkArea.Height) {
                this.Height = this._appData.WindowSizeH;
            }

            if (0 < this._appData.RecentFiles.Count) {
                this.ShowNameList(this._appData.RecentFiles[0], false);
            }

            this.cEnglishNameTitle.Content = Titles.EnglishName + Titles.Asc;
            this.cEnglishNameTitle.Width = this.Width / 3;
            this.cJapaneseNameTitle.Width = this.Width / 3;
            this.cNoteTitle.Width = this.Width / 4;
        }
        #endregion

        #region Event
        /// <summary>
        /// キー入力
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_KeyDown(object sender, KeyEventArgs e) {
            switch(e.Key) {
                case Key.S:
                    if (Keyboard.Modifiers == ModifierKeys.Control) {
                        e.Handled = true;
                        this.RunSaveProcess();
                    }
                    break;
                case Key.W:
                    e.Handled = true;
                    this.Close();
                    break;
            }
        }

        /// <summary>
        /// ウィンドウクローズ
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            if (this._isChanged && this.ShowSaveConfirmDialog()) {
                if (!this.RunSaveProcess()) {
                    e.Cancel = true;
                    return;
                }
            }
            this._appData.WindowPosX = this.Left;
            this._appData.WindowPosY = this.Top;
            this._appData.WindowSizeW = this.Width;
            this._appData.WindowSizeH = this.Height;
            this._appData.Save();
        }



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
                    this.RunSaveProcess(true);
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

        /// <summary>
        /// 名称リストヘッダークリック
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void NameListHeader_Click(object sender, RoutedEventArgs e) {
            var header = (GridViewColumnHeader)sender;
            var title = header.Content.ToString();
            if (title == Titles.EnglishName) {
                this._operator.SortByEnglshName();
                title += Titles.Asc;
                this.cJapaneseNameTitle.Content = Titles.JapaneseName;
            } else if (title == Titles.JapaneseName) {
                this._operator.SortByJapaneseName();
                title += Titles.Asc;
                this.cEnglishNameTitle.Content = Titles.EnglishName;
            } else {
                bool isAsc = title.EndsWith(Titles.Asc);
                if (title.StartsWith(Titles.EnglishName)) {
                    title = Titles.EnglishName;
                    this._operator.SortByEnglshName(!isAsc);
                    this.cJapaneseNameTitle.Content = Titles.JapaneseName;
                } else {
                    title = Titles.EnglishName;
                    this._operator.SortByJapaneseName(!isAsc);
                    this.cEnglishNameTitle.Content = Titles.EnglishName;
                }
                title += isAsc ? Titles.Desc : Titles.Asc;
            }
            header.Content = title;
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
                dialog.FilterIndex = 1;
                if (true == dialog.ShowDialog()) {
                    this._currentNameListFile = dialog.FileName;
                    this._operator.FilePath = this._currentNameListFile;
                    this.AddRecentFile(this._currentNameListFile);
                } else {
                    return false;
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
        /// 名称ファイルを開く。
        /// </summary>
        private void RunOpenProcess() {
            var dialog = new OpenFileDialog();
            dialog.Title = Titles.Open;
            dialog.FileName = Others.NewFileName;
            dialog.Filter = "MNL ファイル|*" + Others.MyNameListExt;
            dialog.FilterIndex = 1;
            if (true == dialog.ShowDialog()) {
                this.ShowNameList(dialog.FileName);
            } else {
                return;
            }
        }

        /// <summary>
        /// 指定された名称リストのファイルを表示する
        /// </summary>
        /// <param name="filePath">名称リストのファイル</param>
        /// <param name="showReadErrorDialog">true:エラーダイアログを表示、false:それ以外</param>
        private void ShowNameList(string filePath, bool showReadErrorDialog = true) {
            this.ClearInputArea();

            if (!_operator.FileLoad(filePath)) {
                if (showReadErrorDialog) {
                    new CommonErrorDialog(this) {
                        ErrorMessage = Messages.FailToOpen,
                        FilePath = filePath
                    }.ShowDialog();
                }
                this._appData.RecentFiles.Remove(filePath);
                this._appData.Save();
                this.CreateRecentFilesMenu();
            } else {
                this.AddRecentFile(filePath);
                this._currentNameListFile = filePath;
            }
        }

        /// <summary>
        /// 最近使ったファイルの追加
        /// </summary>
        /// <param name="filePath"></param>
        private void AddRecentFile(string filePath) {
            var recentFiles = this._appData.RecentFiles;
            if (recentFiles.Contains(filePath)) {
                recentFiles.Remove(filePath);
            }
            recentFiles.Insert(0, filePath);
            while (MaxRecentFileCount < recentFiles.Count) {
                recentFiles.RemoveAt(recentFiles.Count - 1);
            }
            this._appData.Save();
            this.CreateRecentFilesMenu();
        }

        /// <summary>
        /// 最近使ったファイルのメニューを作成
        /// </summary>
        private void CreateRecentFilesMenu() {
            this.cFileMenuRecent.Items.Clear();
            if (0 == this._appData.RecentFiles.Count) {
                this.cFileMenuRecent.IsEnabled = false;
                return;
            }

            this.cFileMenuRecent.IsEnabled = true;
            foreach (var file in this._appData.RecentFiles) {
                var item = new MenuItem() { Header = file };
                item.Click += (sender, e) => {
                    var menuItem = (MenuItem)sender;
                    if (this._isChanged && this.ShowSaveConfirmDialog()) {
                        if (!this.RunSaveProcess()) {
                            return;
                        }
                    }
                    this.ShowNameList(menuItem.Header.ToString());
                };
                this.cFileMenuRecent.Items.Add(item);
            }
        }
        #endregion

    }
}
