using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using System.Windows.Input;

namespace CharCount {
    class MainModelView : INotifyPropertyChanged {
        public MainModelView() {
            CharCountData = new ObservableCollection<CharCountData>();
            CharCountView = new CollectionViewSource();
            CharCountView.Source = CharCountData;
            CharCountView.SortDescriptions.Add(new SortDescription("TheCount", ListSortDirection.Descending));
            resetList();
            IsAutoUpdate = true;
        }
        public event PropertyChangedEventHandler PropertyChanged;
        private delegate void updateDelegate();
        private string _enteredText;
        public string EnteredText {
            get { return _enteredText; }
            set {
                _enteredText = value;
                OnPropertyChanged("EnteredText");
                CountChars();
            }
        }
        public CollectionViewSource CharCountView { get; set; }
        public ObservableCollection<CharCountData> CharCountData { get; private set; }
        private bool? _isAutoUpdate;
        public bool? IsAutoUpdate {
            get
            {
                return _isAutoUpdate ?? false; 
            }
            set
            {
                _isAutoUpdate = value;
                OnPropertyChanged("IsAutoUpdate");
            }
        }
        public DelegateCommand<string> UpdateCommand {
            get
            {
                return new DelegateCommand<string>((param) => countChars_Imp(param));
            }
        }
        protected void OnPropertyChanged(string name) {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        private void resetList() {
            CharCountData.Clear();
            for(var i = 65; i < 91; i++) {
                CharCountData.Add(new CharCountData((char)i, 0));
            }
        }
        private void CountChars() {
            if(!IsAutoUpdate??false) {
                return;
            }
            countChars_Imp(_enteredText);
        }

        private void countChars_Imp(string enteredText) {
            resetList();
            foreach(var letter in _enteredText) {
                if(char.IsLetter(letter)) {
                    if(letter > 91) {
                        CharCountData[letter - 97].TheCount++;
                    } else {
                        CharCountData[letter - 65].TheCount++;
                    }

                }
            }
            CharCountView.View.Refresh();
            OnPropertyChanged("CharCountData");
        }
        
    }


    //https://social.technet.microsoft.com/wiki/contents/articles/18199.event-handling-in-an-mvvm-wpf-application.aspx
    public class DelegateCommand<T> : ICommand where T : class {
        private readonly Predicate<T> _canExecute;
        private readonly Action<T> _execute;

        public DelegateCommand(Action<T> execute)
            : this(execute, null) {
        }

        public DelegateCommand(Action<T> execute, Predicate<T> canExecute) {
            _execute = execute;
            _canExecute = canExecute;
        }

        public bool CanExecute(object parameter) {
            if(_canExecute == null)
                return true;

            return _canExecute((T)parameter);
        }

        public void Execute(object parameter) {
            _execute((T)parameter);
        }

        public event EventHandler CanExecuteChanged;
        public void RaiseCanExecuteChanged() {
            CanExecuteChanged?.Invoke(this, EventArgs.Empty);
        }
    }

}
