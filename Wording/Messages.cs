using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNameList.Wording {
    /// <summary>
    /// メッセージなどを定義
    /// </summary>
    internal class Messages {

        /// <summary>
        /// 不明なメニューアイテム選択。
        /// </summary>
        internal const string UnknownMenuItem = "Unknown MenuItem Clicked.";

        /// <summary>
        /// ファイルの保存失敗
        /// </summary>
        internal const string FailToSave = "Fail to save.";

        /// <summary>
        /// ファイルの保存確認
        /// </summary>
        internal const string ConfirmSave = "Your changes are not saved. Save file?";
    }
}
