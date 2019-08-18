using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyNameList.Data {
    /// <summary>
    /// 名称モデル。
    /// </summary>
    internal class NameModel {

        #region Public Property
        /// <summary>
        /// 英名。
        /// </summary>
        public string EnglishName { set; get; } = "";

        /// <summary>
        /// 和名。
        /// </summary>
        public string JapaneseName { set; get; } = "";

        /// <summary>
        /// メモ。
        /// </summary>
        public string Note { set; get; } = "";
        #endregion
    }
}
