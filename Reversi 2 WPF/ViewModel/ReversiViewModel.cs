
using Reversi_WPF.Model;

using System;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace Reversi_WPF.ViewModel
{
    public class ReversiViewModel : ViewModelBase
    {
        private readonly Int32 _applicationDefaultMinimumHeight = 190;

        #region Fields

        private ReversiGameModel _model;

        private Boolean _isPlayer1TurnOn;

        private Int32 _applicationMinimumHeight;

        private Boolean _saveMenuItemEnabled;

        private Boolean _passButtonEnabled;
        private Boolean _pauseButtonEnabled;

        private String _pauseText;

        private Int32 _player1Time;
        private Int32 _player2Time;

        private String _gamePoints;

        private Boolean _saved;

        #endregion

        #region Properties

        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand LoadGameCommand { get; private set; }

        public DelegateCommand SaveGameCommand { get; private set; }

        public DelegateCommand ExitApplicationCommand { get; private set; }

        public DelegateCommand RulesCommand { get; private set; }

        public DelegateCommand AboutCommand { get; private set; }

        public DelegateCommand PassCommand { get; private set; }

        public DelegateCommand PauseCommand { get; private set; }

        public ObservableCollection<ReversiCell> Cells { get; private set; }

        public Int32 ApplicationHeight
        {
            get; private set;
        }

        public Int32 ApplicationWidth
        {
            get; private set;
        }

        public Int32 ApplicationMinimumHeight { get { return _applicationMinimumHeight; } }

        public Boolean SaveMenuItemEnabled { get { return _saveMenuItemEnabled; } }

        public Boolean SmallMenuItemEnabled
        {
            get
            {
                return !(SmallMenuItemChecked);
            }
        }

        public Boolean MediumMenuItemEnabled
        {
            get
            {
                return !(MediumMenuItemChecked);
            }
        }

        public Boolean LargeMenuItemEnabled
        {
            get
            {
                return !(LargeMenuItemChecked);
            }
        }

        public Boolean SmallMenuItemChecked
        {
            get
            {
                return _model.TableSizeSetting == 10;
            }
            set
            {
                _model.TableSizeSetting = 10;
                OnPropertyChanged("SmallMenuItemChecked");
                OnPropertyChanged("MediumMenuItemChecked");
                OnPropertyChanged("LargeMenuItemChecked");
                OnPropertyChanged("SmallMenuItemEnabled");
                OnPropertyChanged("MediumMenuItemEnabled");
                OnPropertyChanged("LargeMenuItemEnabled");
            }
        }

        public Boolean MediumMenuItemChecked
        {
            get
            {
                return _model.TableSizeSetting == 20;
            }
            set
            {
                _model.TableSizeSetting = 20;
                OnPropertyChanged("SmallMenuItemChecked");
                OnPropertyChanged("MediumMenuItemChecked");
                OnPropertyChanged("LargeMenuItemChecked");
                OnPropertyChanged("SmallMenuItemEnabled");
                OnPropertyChanged("MediumMenuItemEnabled");
                OnPropertyChanged("LargeMenuItemEnabled");
            }
        }

        public Boolean LargeMenuItemChecked
        {
            get
            {
                return _model.TableSizeSetting == 30;
            }
            set
            {
                _model.TableSizeSetting = 30;
                OnPropertyChanged("SmallMenuItemChecked");
                OnPropertyChanged("MediumMenuItemChecked");
                OnPropertyChanged("LargeMenuItemChecked");
                OnPropertyChanged("SmallMenuItemEnabled");
                OnPropertyChanged("MediumMenuItemEnabled");
                OnPropertyChanged("LargeMenuItemEnabled");
            }
        }

        public Boolean PassButtonEnabled { get { return _passButtonEnabled; } }

        public Boolean PauseButtonEnabled { get { return _pauseButtonEnabled; } }

        public String PauseText { get { return _pauseText; } }

        public Int32 Player1Time { get { return _player1Time; } }

        public Int32 Player2Time { get { return _player2Time; } }

        public String GamePoints { get { return _gamePoints; } }

        public Boolean Saved { get { return _saved; } set { _saved = value; } }

        #endregion

        #region Events

        public event EventHandler NewGame;

        public event EventHandler LoadGame;

        public event EventHandler SaveGame;

        public event EventHandler ExitApplication;

        public event EventHandler ReadRules;

        public event EventHandler ReadAbout;

        #endregion

        #region Constructors

        /// <summary>
        /// Sudoku nézetmodell példányosítása.
        /// </summary>
        /// <param name="model">A modell típusa.</param>
        public ReversiViewModel(ReversiGameModel model)
        {
            

            _model = model;
            _model.SetGameEnded += new EventHandler<ReversiSetGameEndedEventArgs>(Model_SetGameEnded);
            _model.UpdatePlayerTime += new EventHandler<ReversiUpdatePlayerTimeEventArgs>(Model_UpdatePlayerTime);
            _model.UpdateTable += new EventHandler<ReversiUpdateTableEventArgs>(Model_UpdateTable);

            NewGameCommand = new DelegateCommand(param => { OnNewGame(); });
            LoadGameCommand = new DelegateCommand(param => { OnLoadGame(); });
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitApplicationCommand = new DelegateCommand(param => OnExitApplication());

            RulesCommand = new DelegateCommand(param => OnReadRules());
            AboutCommand = new DelegateCommand(param => OnReadAbout());

            PassCommand = new DelegateCommand(param => OnPass());
            PauseCommand = new DelegateCommand(param => OnPause());

            Cells = new ObservableCollection<ReversiCell>();

            _applicationMinimumHeight = _applicationDefaultMinimumHeight;
            OnPropertyChanged("ApplicationMinimumHeight");

            _saved = true;

            _saveMenuItemEnabled = false;
            OnPropertyChanged("SaveMenuItemEnabled");

            _passButtonEnabled = false;
            OnPropertyChanged("PassButtonEnabled");
            _pauseButtonEnabled = false;
            OnPropertyChanged("PauseButtonEnabled");

            _pauseText = "Pause";
            OnPropertyChanged("PauseText");

            _player1Time = 0;
            OnPropertyChanged("Player1Time");
            _player2Time = 0;
            OnPropertyChanged("Player2Time");

            _gamePoints = "";
            OnPropertyChanged("GamePoints");
        }

        #endregion

        #region Private methods

        private void MakePutDown(Int32 index)
        {
            ReversiCell cell = Cells[index];

            if (cell.Enabled == true)
            {
                _model.PutDown(cell.X, cell.Y);
            }
        }

        public void SetButtonGridUp()
        {
            if (Cells.Count == 0 || Cells.Count != _model.ActiveTableSize * _model.ActiveTableSize)
            {
                Cells.Clear();
                for (Int32 i = 0; i < _model.ActiveTableSize; ++i)
                {
                    for (Int32 j = 0; j < _model.ActiveTableSize; ++j)
                    {
                        Cells.Add(new ReversiCell(new DelegateCommand(param => MakePutDown(Convert.ToInt32(param))), i, j, ((i * _model.ActiveTableSize) + j)));
                    }
                }
                
            }
        }

        public void UpdateCell(Int32 x, Int32 y, Int32 value)
        {
            Int32 index = ((x * _model.ActiveTableSize) + y);
            ReversiCell cell = Cells[index];

            switch (value)
            {
                case -1:
                    cell.Text = "";
                    cell.BackColorInt = 0; // Color.White;
                    cell.Enabled = false;
                    break;

                case 0:
                    cell.Text = "";
                    cell.BackColorInt = 2; // Color.YellowGreen;
                    cell.Enabled = false;
                    break;

                case 1:
                    cell.Text = "";
                    cell.BackColorInt = 1; // Color.Black;
                    cell.Enabled = false;
                    break;

                case 3:
                    if (!_isPlayer1TurnOn)
                    {
                        cell.Text = "o";
                        cell.TextColorInt = 1; // Color.Black;
                        cell.Enabled = true;
                    }
                    else
                    {
                        cell.Text = "";

                        cell.Enabled = false;
                    }

                    cell.BackColorInt = 2; // Color.YellowGreen;

                    break;

                case 6:

                    if (_isPlayer1TurnOn)
                    {
                        cell.Text = "o";
                        cell.TextColorInt = 0; // Color.White;
                        cell.Enabled = true;
                    }
                    else
                    {
                        cell.Text = "";

                        cell.Enabled = false;
                    }

                    cell.BackColorInt = 2; // Color.YellowGreen;

                    break;

                case 4:
                    cell.Text = "o";
                    if (_isPlayer1TurnOn)
                    {
                        cell.TextColorInt = 0; // Color.White;
                    }
                    else
                    {
                        cell.TextColorInt = 1; // Color.Black;
                    }
                    //cell.TextColorInt = 3 // Color.Gray;
                    cell.Enabled = true;
                    break;

                case 5:
                    cell.Text = "";
                    cell.BackColorInt = 2; // Color.YellowGreen;
                    cell.Enabled = false;

                    break;

                default:
                    throw new Exception("Model gave us a number, that we was not ready for, while updating the table view.");
            }
        }

        #endregion

        #region Game event handlers

        private void Model_SetGameEnded(object sender, ReversiSetGameEndedEventArgs e)
        {
            _passButtonEnabled = false;
            OnPropertyChanged("PassButtonEnabled");
            _pauseButtonEnabled = false;
            OnPropertyChanged("PauseButtonEnabled");

            _saved = true;
            _saveMenuItemEnabled = false;
            OnPropertyChanged("SaveMenuItemEnabled");
        }

        private void Model_UpdatePlayerTime(object sender, ReversiUpdatePlayerTimeEventArgs e)
        {
            if (e.IsPlayer1TimeNeedUpdate)
            {
                _player1Time = e.NewTime;
                OnPropertyChanged("Player1Time");
            }
            else
            {
                _player2Time = e.NewTime;
                OnPropertyChanged("Player2Time");
            }
        }

        private void Model_UpdateTable(object sender, ReversiUpdateTableEventArgs e)
        {
            _saved = false;
            _saveMenuItemEnabled = true;
            OnPropertyChanged("SaveMenuItemEnabled");
            


            _gamePoints = "Player 1: " + e.Player1Points + " -- Player 2: " + e.Player2Points;
            OnPropertyChanged("GamePoints");

            _isPlayer1TurnOn = e.IsPlayer1TurnOn;

            _passButtonEnabled = e.IsPassingTurnOn;
            OnPropertyChanged("PassButtonEnabled");

            if (e.UpdatedFieldsCount == 0)
            {
                _pauseButtonEnabled = true;
                OnPropertyChanged("PauseButtonEnabled");

                _pauseText = "Pause";
                OnPropertyChanged("PauseText");

                SetButtonGridUp();

                Int32 index = 0;
                for (Int32 x = 0; x < _model.ActiveTableSize; ++x)
                {
                    for (Int32 y = 0; y < _model.ActiveTableSize; ++y)
                    {
                        UpdateCell(x, y, e.UpdatedFieldsDatas[index]);
                        ++index;
                    }
                }
            }
            else
            {
                for (Int32 i = 0; i < e.UpdatedFieldsCount; i += 3)
                {
                    UpdateCell(e.UpdatedFieldsDatas[i], e.UpdatedFieldsDatas[i + 1], e.UpdatedFieldsDatas[i + 2]);
                }
            }
        }

        #endregion

        #region Event methods

        private void OnNewGame()
        {
            if (NewGame != null)
            {
                NewGame(this, EventArgs.Empty);
            }
        }

        private void OnLoadGame()
        {
            if (LoadGame != null)
            {
                LoadGame(this, EventArgs.Empty);
                if (_saved == false)
                {
                    _saveMenuItemEnabled = true;
                    OnPropertyChanged("SaveMenuItemEnabled");
                }
            }
        }

        private void OnSaveGame()
        {
            if (SaveGame != null)
            {
                SaveGame(this, EventArgs.Empty);
                if (_saved == true)
                {
                    _saveMenuItemEnabled = false;
                    OnPropertyChanged("SaveMenuItemEnabled");
                }
            }
        }

        private void OnExitApplication()
        {
            if (ExitApplication != null)
            {
                ExitApplication(this, EventArgs.Empty);
            }
        }

        private void OnReadRules()
        {
            if (ReadRules != null)
            {
                ReadRules(this, EventArgs.Empty);
            }
        }

        private void OnReadAbout()
        {
            if (ReadAbout != null)
            {
                ReadAbout(this, EventArgs.Empty);
            }
        }

        private void OnPass()
        {
            _model.Pass();
            _saved = false;
            _saveMenuItemEnabled = true;
            OnPropertyChanged("SaveMenuItemEnabled");
        }

        private void OnPause()
        {
            if (_pauseText == "Pause")
            {
                _pauseText = "Unpause";
                _model.Pause();
                OnPropertyChanged("PauseText");
            }
            else
            {
                _pauseText = "Pause";
                _model.Unpause();
                OnPropertyChanged("PauseText");
            }
        }

        #endregion
    }
}
