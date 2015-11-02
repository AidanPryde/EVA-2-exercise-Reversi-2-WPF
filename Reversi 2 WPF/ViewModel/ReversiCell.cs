
using System;

namespace Reversi_WPF.ViewModel
{

    /// <summary>
    /// The reversi game-cell class.
    /// </summary>
    public class ReversiCell : ViewModelBase
    {

        #region Fields

        private Boolean _enabled;
        private String _text;
        private Int32 _textColorInt;
        private Int32 _backColorInt;

        private Int32 _x;
        private Int32 _y;

        private Int32 _index;

        private DelegateCommand _putDownCommand;

        #endregion

        #region Properties

        public Boolean Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                if (_enabled != value)
                {
                    _enabled = value;
                    OnPropertyChanged();
                }
            }
        }

        public String Text
        {
            get
            {
                return _text;
            }

            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 TextColorInt
        {
            get
            {
                return _textColorInt;
            }

            set
            {
                if (_textColorInt != value)
                {
                    _textColorInt = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 BackColorInt
        {
            get
            {
                return _backColorInt;
            }

            set
            {
                if (_backColorInt != value)
                {
                    _backColorInt = value;
                    OnPropertyChanged();
                }
            }
        }

        public Int32 X
        {
            get
            {
                return _x;
            }
        }

        public Int32 Y
        {
            get
            {
                return _y;
            }
        }
        public Int32 Index
        {
            get
            {
                return _index;
            }
        }

        public DelegateCommand PutDownCommand
        {
            get
            {
                return _putDownCommand;
            }
        }

        #endregion

        #region Constructors

        public ReversiCell(DelegateCommand putDown, Int32 x, Int32 y, Int32 index, Boolean enabled = false, String text = "", Int32 textColorInt = 0, Int32 backColorInt = 0)
        {
            _putDownCommand = putDown;

            _x = x;
            _y = y;

            _index = index;

            _enabled = enabled;
            _text = text;
            _textColorInt = textColorInt;
            _backColorInt = backColorInt;
        }

        #endregion

    }
}
