using System.ComponentModel;

namespace MyNameList.Data {
    /// <summary>
    /// 名称モデル。
    /// </summary>
    internal class NameModel : INotifyPropertyChanged {

        #region Declaration
        public event PropertyChangedEventHandler PropertyChanged;

        private string _englishName = "";
        private string _japaneseName = "";
        private string _note = "";
        #endregion

        #region Public Property
        /// <summary>
        /// 英名。
        /// </summary>
        public string EnglishName {
            set {
                this._englishName = value;
                this.OnPropertyChanged(nameof(EnglishName));
            }
            get { return this._englishName; }
        }

        /// <summary>
        /// 和名。
        /// </summary>
        public string JapaneseName {
            set {
                this._japaneseName = value;
                this.OnPropertyChanged(nameof(JapaneseName));
            }
            get { return this._japaneseName; }
        }

        /// <summary>
        /// メモ。
        /// </summary>
        public string Note {
            set {
                this._note = value;
                this.OnPropertyChanged(nameof(Note));
            }
            get { return this._note; }
        }
        #endregion

        #region Constructor
        public NameModel() { }
        public NameModel(NameModel model) {
            this.EnglishName = model.EnglishName;
            this.JapaneseName = model.JapaneseName;
            this.Note = model.Note;
        }
        #endregion

        #region Protecte Method
        protected void OnPropertyChanged(string name) {
            PropertyChangedEventHandler handler = PropertyChanged;
            handler?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        #endregion
    }
}
