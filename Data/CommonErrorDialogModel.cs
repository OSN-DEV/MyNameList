using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNameList.Data {
    /// <summary>
    /// 汎用エラーダイアログモデル
    /// </summary>
    public class CommonErrorDialogModel {

        #region Public Property
        /// <summary>
        /// ダイアログのタイトル。省略時は"Error"を設定。
        /// </summary>
        public string DialogTitle { set; get; } = "Error";

        /// <summary>
        /// エラーメッセージ
        /// </summary>
        public string ErrorMessage { set; get; } = "";

        /// <summary>
        /// エラーの発生したファイルのフルパス
        /// </summary>
        public string FilePath { set; get; } = "";
        #endregion
    }
}
