using System.Collections.Generic;
using MyLib.Data;
using MyNameList.Util;

namespace MyNameList.Data {
    /// <summary>
    /// アプリケーションデータ。
    /// </summary>
    public class AppData : AppDataBase<AppData> {

        #region Declaration
        private static readonly string FileName = "app.data";
        #endregion

        #region Public Property
        /// <summary>
        /// 最後にアプリを表示していた時のウィンドウのサイズ(幅)。
        /// </summary>
        public double WindowSizeW { set; get; } = -1;

        /// <summary>
        /// 最後にアプリを表示していた時のウィンドウのサイズ(高さ)。
        /// </summary>
        public double WindowSizeH { set; get; } = -1;

        /// <summary>
        /// 最後にアプリを表示していた時のウィンドウの位置(X座標)。
        /// </summary>
        public double WindowPosX { set; get; } = -1;

        /// <summary>
        /// 最後にアプリを表示していた時のウィンドウの位置(Y座標)。
        /// </summary>
        public double WindowPosY { set; get; } = -1;

        /// <summary>
        /// 最近使ったファイルのリスト。
        /// </summary>
        public List<string> RecentFiles { set; get; } = new List<string>();
        #endregion

        #region Public Method
        /// <summary>
        /// AppDataのインスタンスを取得する。
        /// </summary>
        /// <returns>AppDataのインスタンス</returns>
        internal static AppData GetInstance() {
            if (null == AppDataBase<AppData>._instance) {
                return AppDataBase<AppData>.GetInstanceBase(AppCommon.GetAppPath() + FileName);
            } else {
                return AppDataBase<AppData>.GetInstanceBase();
            }
        }

        /// <summary>
        /// AppDataの情報を保存する。
        /// </summary>
        internal void Save() {
            base.SaveToXml();
        }
        #endregion
    }
}
