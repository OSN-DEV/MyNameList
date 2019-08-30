using MyLib.File;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace MyNameList.Data {
    /// <summary>
    /// 名称リスト操作クラス。
    /// </summary>
    internal class NameListOperator {
        #region Declaration
        private const char Separator = '\t';
        /// <summary>
        /// ソート種別
        /// </summary>
        private enum SortType {
            /// <summary>
            /// 英名昇順
            /// </summary>
            EnAsc,
            /// <summary>
            /// 英名降順
            /// </summary>
            EnDesc,
            /// <summary>
            /// 和名昇順
            /// </summary>
            JpAsc,
            /// <summary>
            /// 和名降順
            /// </summary>
            JpDesc
        }
        private SortType _currentSortType = SortType.EnAsc;
        #endregion

        #region Constructor
        #endregion;

        #region Public Property
        /// <summary>
        /// リストのデータソース。
        /// </summary>
        internal ObservableCollection<NameModel> DataContext { private set;  get; } = new ObservableCollection<NameModel>();

        /// <summary>
        /// 名称リストのファイルパス
        /// </summary>
        internal string FilePath { set; get; } = null;

        /// <summary>
        /// 名称リストのファイル名
        /// </summary>
        internal string FileName { private set; get; } = null;
        #endregion

        #region Public Method
        /// <summary>
        /// 名称リストのファイルを読込む。
        /// </summary>
        /// <param name="filePath">読込対象となるファイル</param>
        /// <returns>true:読込成功、false:それ以外</returns>
        /// <remarks>読込に成功した場合は情報を`DataContext`に設定する。  
        /// 最初に`DataContext`をクリアするので読込失敗時は`DataContext`が空になる。</remarks>
        internal bool FileLoad(string filePath) {
            this.DataContext.Clear();
            using (var file = new FileOperator(filePath)) {
                if (!file.Exists()) {
                    return false;
                }
                file.OpenForRead();
                while(file.Eof) {
                    var item = file.ReadLine().Split(Separator);
                    Array.Resize(ref item, 3);
                    this.DataContext.Add(new NameModel() {
                        EnglishName = item[0],
                        JapaneseName = item[1],
                        Note = item[2]
                    });
                }
                this.FileName = file.Name;
            }
            this.FilePath = filePath;
            this._currentSortType = SortType.EnAsc;
            this.SortBySortType();
            return false;
        }

        /// <summary>
        /// 名称リストのファイルを保存する。
        /// </summary>
        /// <returns>true:保存成功、false:それ以外</returns>
        internal bool Save() {
            using (var file = new FileOperator(this.FilePath)) {
                try {
                    file.Delete();
                    file.OpenForWrite();
                    foreach (var model in this.DataContext) {
                        file.WriteLine(model.EnglishName + Separator +
                                       model.JapaneseName + Separator +
                                       model.Note);
                    }
                    this.FileName = file.Name;
                } catch {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 名称リストにモデルを追加する。
        /// </summary>
        /// <param name="englishName">英名</param>
        /// <param name="japaneseName">和名</param>
        /// <param name="note">メモ</param>
        internal void Add(string englishName, string japaneseName, string note) {
            this.DataContext.Add(new NameModel() {
                EnglishName = englishName,
                JapaneseName = japaneseName,
                Note = note
            });
            this.SortBySortType();
        }

        /// <summary>
        /// 名称リストのモデルを位置を指定して削除する。
        /// </summary>
        /// <param name="index">削除するモデルのインデックス</param>
        internal void RemoveAt(int index) {
            this.DataContext.RemoveAt(index);
        }

        /// <summary>
        /// 英名でリストをソートする。
        /// </summary>
        /// <param name="ascending">true:昇順でソートする、false:降順でソートする</param>
        internal void SortByEnglshName(bool ascending = true) {
            this._currentSortType = ascending ? SortType.EnAsc : SortType.EnDesc;
            if (ascending) {
                this.DataContext = new ObservableCollection<NameModel>(this.DataContext.OrderBy(n => n.EnglishName));
            } else {
                this.DataContext = new ObservableCollection<NameModel>(this.DataContext.OrderByDescending(n => n.EnglishName));
            }
        }

        /// <summary>
        /// 和名でリストをソートする。
        /// </summary>
        /// <param name="ascending">true:昇順でソートする、false:降順でソートする</param>
        internal void SortByJapaneseName(bool ascending = true) {
            this._currentSortType = ascending ? SortType.JpAsc : SortType.JpDesc;
            if (ascending) {
                this.DataContext = new ObservableCollection<NameModel>(this.DataContext.OrderBy(n => n.JapaneseName));
            } else {
                this.DataContext = new ObservableCollection<NameModel>(this.DataContext.OrderByDescending(n => n.JapaneseName));
            }
        }
        #endregion

        #region Private Method
        /// <summary>
        /// 現在のソート種別に準じてソートを行う
        /// </summary>
        private void SortBySortType() {
            switch(this._currentSortType) {
                case SortType.EnAsc:
                case SortType.EnDesc:
                    this.SortByEnglshName(this._currentSortType == SortType.EnAsc);
                    break;
                default:
                    this.SortByJapaneseName(this._currentSortType == SortType.JpAsc);
                    break;
            }
        }
        #endregion

    }
}
