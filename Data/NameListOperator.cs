using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNameList.Data {
    /// <summary>
    /// 名称リスト操作クラス。
    /// </summary>
    internal class NameListOperator {

        #region Public Property
        /// <summary>
        /// リストのデータソース。
        /// </summary>
        internal ObservableCollection<NameModel> DataContext { get; } = null;
        #endregion

        #region Public Method
        /// <summary>
        /// 名称リストのファイルを読込む。
        /// </summary>
        /// <param name="filePath">読込対象となるファイル</param>
        /// <returns>true:読込成功、false:それ以外</returns>
        /// <remarks>読込に成功した場合は情報を`DataContext`に設定する。  
        /// 最初に`DataContext`を`null`クリアするので読込失敗時は`DataContext`が`null`になる。</remarks>
        internal bool FileLoad(string filePath) {
            return false;
        }

        /// <summary>
        /// 名称リストのファイルを保存する。
        /// </summary>
        /// <returns>true:保存成功、false:それ以外</returns>
        internal bool Save() {
            return true;
        }

        /// <summary>
        /// 名称リストにモデルを追加する。
        /// </summary>
        /// <param name="englishName">英名</param>
        /// <param name="japaneseName">和名</param>
        /// <param name="note">メモ</param>
        internal void Add(string englishName, string japaneseName, string note) {

        }

        /// <summary>
        /// 名称リストのモデルを位置を指定して削除する。
        /// </summary>
        /// <param name="index">削除するモデルのインデックス</param>
        internal void RemoveAt(int index) {

        }

        /// <summary>
        /// 英名でリストをソートする。
        /// </summary>
        /// <param name="ascending">true:昇順でソートする、false:降順でソートする</param>
        internal void SortByEnglshName(bool ascending = true) {

        }

        /// <summary>
        /// 和名でリストをソートする。
        /// </summary>
        /// <param name="ascending">true:昇順でソートする、false:降順でソートする</param>
        internal void SortByJapaneseName(bool ascending = true) {
        }
        #endregion

    }
}
