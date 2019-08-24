using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace MyNameList.Util {
    /// <summary>
    /// アプリ汎用メソッド
    /// </summary>
    internal class AppCommon {

        #region Public Method
        /// <summary>
        /// アプリの実行パスを取得する。
        /// </summary>
        /// <returns>アプリの実行パス</returns>
        internal static string GetAppPath() {
            var path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            if (path.EndsWith(@"\")) {
                return path;
            } else {
                return path + @"\";
            }
        }


        /// <summary>
        /// バージョン付きアプリ名称を取得する。
        /// </summary>
        /// <returns>バージョン付きアプリ名称</returns>
        internal static string GetAppName() {
            var fullname = typeof(App).Assembly.Location;
            var info = System.Diagnostics.FileVersionInfo.GetVersionInfo(fullname);
            var ver = info.FileVersion;
            return $"{Wording.Title.AppName}({ver})";
        }


        /// <summary>
        /// デバッグログを出力する。
        /// </summary>
        /// <param name="log">ログメッセージ</param>
        /// <param name="file">呼び出し元のファイル名</param>
        /// <param name="line">呼び出し元の行番号</param>
        /// <param name="member">呼び出し元のメンバー名(メソッド・プロパティ・イベント名等)</param>
        internal static void DebugLog(string log, 
                [CallerFilePath] string file = "", 
                [CallerLineNumber] int line = 0, 
                [CallerMemberName] string member = "") {
            System.Diagnostics.Debug.WriteLine($"[{file}][{line}][{member}]{log}");
        }
        #endregion
    }
}
