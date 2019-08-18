using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNameList.Data {
    /// <summary>
    /// アプリケーションデータ。
    /// </summary>
    internal class AppData : MyLib.Data.AppDataBase<AppData>{

        #region Public Property
        /// <summary>
        /// 最後にアプリを表示していた時のウィンドウのサイズ(幅)。
        /// </summary>
        internal double WindowSizeW { set; get; } = -1;

        /// <summary>
        /// 最後にアプリを表示していた時のウィンドウのサイズ(高さ)。
        /// </summary>
        internal double WindowSizeH { set; get; } = -1;

        /// <summary>
        /// 最後にアプリを表示していた時のウィンドウの位置(X座標)。
        /// </summary>
        internal double WindowPosX { set; get; } = -1;

        /// <summary>
        /// 最後にアプリを表示していた時のウィンドウの位置(Y座標)。
        /// </summary>
        internal double WindowPosY { set; get; } = -1;

        /// <summary>
        /// 最近使ったファイルのリスト。
        /// </summary>
        internal List<string> RecentFiles { set; get; } = new List<string>();
        #endregion

        #region Public Method
        /// <summary>
        /// AppDataのインスタンスを取得する。
        /// </summary>
        /// <returns>AppDataのインスタンス</returns>
        internal static AppData GetInstance() {
            return null;
        }

        /// <summary>
        /// AppDataの情報を保存する。
        /// </summary>
        internal void Save() {

        }
        #endregion
    }
}
