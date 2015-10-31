
using Reversi_WPF.Model;

using System;
using System.Windows.Threading;
using System.Collections.ObjectModel;

namespace Reversi_WPF.ViewModel
{
    public class ReversiViewModel : ViewModelBase
    {


        #region Fields

        private Dispatcher _dispatcher;
        private ReversiGameModel _model;

        #endregion

        #region Properties

        public DelegateCommand NewGameCommand { get; private set; }

        public DelegateCommand LoadGameCommand { get; private set; }

        public DelegateCommand SaveGameCommand { get; private set; }

        public DelegateCommand ExitCommand { get; private set; }

        public DelegateCommand RulesCommand { get; private set; }

        public DelegateCommand AboutCommand { get; private set; }

        public ObservableCollection<ReversiCell> Cells { get; set; }

        #endregion

        #region Events

        public event EventHandler NewGame;

        public event EventHandler LoadGame;

        public event EventHandler SaveGame;

        public event EventHandler ExitGame;

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
            _model.UpdatePlayerTime += new EventHandler<ReversiUpdatePlayerTimeEventArgs>(Model_UpdatePlayerTime);
            _model.UpdateTable += new EventHandler<ReversiUpdateTableEventArgs>(Model_UpdateTable);

            NewGameCommand = new DelegateCommand(param => { OnNewGame(); RefreshTable(); });
            LoadGameCommand = new DelegateCommand(param => { OnLoadGame(); RefreshTable(); });
            SaveGameCommand = new DelegateCommand(param => OnSaveGame());
            ExitCommand = new DelegateCommand(param => OnExitGame());

            RulesCommand = new DelegateCommand(param => OnExitGame());
            AboutCommand = new DelegateCommand(param => OnExitGame());

            Cells = new ObservableCollection<ReversiCell>();
            for (Int32 i = 0; i < _model.ActiveTableSize; ++i)
            {
                for (Int32 j = 0; j < _model.ActiveTableSize; ++j)
                {
                    Cells.Add(new ReversiCell
                    {
                        IsLocked = true,
                        Text = String.Empty,
                        Color = ConsoleColor.White,
                        X = i,
                        Y = j,
                        PutDownCommand = new DelegateCommand(param => StepGame(Convert.ToInt32(param)))
                    });
                }
            }
        }

        #endregion

        #region Private methods

        private void setButtonGridUp()
        {
        }

        #endregion

        #region Game event handlers

        private void Model_SetGameEnded(object sender, ReversiSetGameEndedEventArgs e)
        {
            
        }

        private void Model_UpdatePlayerTime(object sender, ReversiUpdatePlayerTimeEventArgs e)
        {

        }

        private void Model_UpdateTable(object sender, ReversiUpdateTableEventArgs e)
        {

        }

        #endregion

        #region Event methods

        #endregion
    }
}
