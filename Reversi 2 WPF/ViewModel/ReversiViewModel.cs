
using Reversi_WPF.Model;

using System;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace Reversi_WPF.ViewModel
{
    public class ReversiViewModel : ViewModelBase
    {


        #region Fields

        private ReversiGameModel _model;

        private Boolean _isPlayer1TurnOn;

        private Int32 _applicationMinimumHeight;

        private Boolean _saveMenuItemEnabled;

        private Boolean _smallMenuItemEnabled;
        private Boolean _mediumMenuItemEnabled;
        private Boolean _largeMenuItemEnabled;

        private Boolean _smallMenuItemChecked;
        private Boolean _mediumMenuItemChecked;
        private Boolean _largeMenuItemChecked;

        private Boolean _passButtonEnabled;
        private Boolean _pauseButtonEnabled;

        private Int32 _player1Time;
        private Int32 _player2Time;

        private String _gamePoints;

        #endregion

        #region Properties

        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand LoadGameCommand { get; private set; }

        public DelegateCommand SaveGameCommand { get; private set; }

        public DelegateCommand ExitApplicationCommand { get; private set; }

        public DelegateCommand RulesCommand { get; private set; }

        public DelegateCommand AboutCommand { get; private set; }

        public ObservableCollection<ReversiCell> Cells { get; private set; }

        public Int32 ApplicationMinimumHeight { get { return _applicationMinimumHeight; } private set { _applicationMinimumHeight = value; } }

        public Boolean SaveMenuItemEnabled { get { return _saveMenuItemEnabled; } private set { _saveMenuItemEnabled = value; } }

        public Boolean SmallMenuItemEnabled { get { return _smallMenuItemEnabled; } private set { _smallMenuItemEnabled = value; } }

        public Boolean MediumMenuItemEnabled { get { return _mediumMenuItemEnabled; } private set { _mediumMenuItemEnabled = value; } }

        public Boolean LargeMenuItemEnabled { get { return _largeMenuItemEnabled; } private set { _largeMenuItemEnabled = value; } }

        public Boolean SmallMenuItemChecked { get { return _smallMenuItemChecked; } set { _smallMenuItemChecked = value; } }

        public Boolean MediumMenuItemChecked { get { return _mediumMenuItemChecked; } set { _mediumMenuItemChecked = value; } }

        public Boolean LargeMenuItemChecked { get { return _largeMenuItemChecked; } set { _largeMenuItemChecked = value; } }

        public Boolean PassButtonEnabled { get { return _passButtonEnabled; } set { _passButtonEnabled = value; } }

        public Boolean PauseButtonEnabled { get { return _pauseButtonEnabled; } set { _pauseButtonEnabled = value; } }

        public Int32 Player1Time { get { return _player1Time; } private set { _player1Time = value; } }

        public Int32 Player2Time { get { return _player2Time; } private set { _player2Time = value; } }

        public String GamePoints { get { return _gamePoints; } private set { _gamePoints = value; } }

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

            NewGameCommand = new DelegateCommand(param => { OnNewGame(); /*RefreshTable();*/ });
            LoadGameCommand = new DelegateCommand(param => { OnLoadGame(); /*RefreshTable();*/ });
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitApplicationCommand = new DelegateCommand(param => OnExitApplication());

            RulesCommand = new DelegateCommand(param => OnReadRules());
            AboutCommand = new DelegateCommand(param => OnReadAbout());

            Cells = new ObservableCollection<ReversiCell>();

            _applicationMinimumHeight = 190;
            OnPropertyChanged("ApplicationMinimumHeight");

            _saveMenuItemEnabled = false;
            OnPropertyChanged("SaveMenuItemEnabled");
            _smallMenuItemEnabled = false;
            OnPropertyChanged("SmallMenuItemEnabled");
            _mediumMenuItemEnabled = true;
            OnPropertyChanged("MediumMenuItemEnabled");
            _largeMenuItemEnabled = true;
            OnPropertyChanged("LargeMenuItemEnabled");
            _smallMenuItemChecked = true;
            OnPropertyChanged("SmallMenuItemChecked");
            _mediumMenuItemChecked = false;
            OnPropertyChanged("MediumMenuItemChecked");
            _largeMenuItemChecked = false;
            OnPropertyChanged("LargeMenuItemChecked");
        }

        #endregion

        #region Private methods

        private void MakePutDown(Int32 index)
        {
            ReversiCell cell = Cells[index];

            _model.PutDown(cell.X, cell.Y);
        }

        public void SetButtonGridUp()
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

            //OnPropertyChanged("Cells");
        }

        #endregion

        #region Game event handlers

        private void Model_SetGameEnded(object sender, ReversiSetGameEndedEventArgs e)
        {
            // disable pause and pass buttons and save button
            // it is saved
        }

        private void Model_UpdatePlayerTime(object sender, ReversiUpdatePlayerTimeEventArgs e)
        {
            // update player time
        }

        private void Model_UpdateTable(object sender, ReversiUpdateTableEventArgs e)
        {
            // it is NOT saved

            // update points
            _isPlayer1TurnOn = e.IsPlayer1TurnOn;

            // passing button seting

            if (e.UpdatedFieldsCount == 0)
            {
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
            }
        }

        private void OnSaveGame()
        {
            if (SaveGame != null)
            {
                SaveGame(this, EventArgs.Empty);
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

        #endregion
    }
}
