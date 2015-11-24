
using Reversi_WPF.Model;
using Reversi_WPF.Persistence;
using Reversi_WPF.View;
using Reversi_WPF.ViewModel;

using Microsoft.Win32;

using System;
using System.Windows;
using System.ComponentModel;

namespace Reversi_WPF
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {

        #region Constants

        /// <summary>
        /// Array of the allowed table sizes. It is readonly.
        /// </summary>
        private readonly Int32[] _supportedGameTableSizesArray = new Int32[] { 10, 20, 30 };
        /// <summary>
        /// The default table size. It is readonly.
        /// </summary>
        private readonly Int32 _tableSizeDefaultSetting = 10;

        #endregion

        #region Fields

        private ReversiGameModel _model;
        private ReversiViewModel _viewModel;
        private MainWindow _view;
        private AboutMessage _aboutView;

        #endregion

        #region Constructors

        /// <summary>
        /// The instantiation of the application.
        /// </summary>
        public App()
        {
            Startup += new StartupEventHandler(App_Startup);
        }

        #endregion

        #region Application event handlers

        /// <summary>
        /// Initialization of the application.
        /// </summary>
        /// <param name="sender">This object. We do not use it.</param>
        /// <param name="e">Contains the program parameters. We do not use it.</param>
        private void App_Startup(object sender, StartupEventArgs e)
        {
            _model = new ReversiGameModel(new ReversiFileDataAccess(_supportedGameTableSizesArray), _tableSizeDefaultSetting);
            _model.SetGameEnded += new EventHandler<ReversiSetGameEndedEventArgs>(Model_SetGameEnded);

            _viewModel = new ReversiViewModel(_model);
            _viewModel.NewGame += new EventHandler(ViewModel_NewGame);
            _viewModel.LoadGame += new EventHandler(ViewModel_LoadGame);
            _viewModel.SaveGame += new EventHandler(ViewModel_SaveGame);
            _viewModel.ExitApplication += new EventHandler(ViewModel_ExitApplication);

            _viewModel.ReadRules += new EventHandler(ViewModel_ReadRules);
            _viewModel.ReadAbout += new EventHandler(ViewModel_ReadAbout);

            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new CancelEventHandler(View_Closing);
            _view.Show();
        }

        #endregion

        #region View event handlers

        /// <summary>
        /// Invoked, when we try to close the application.
        /// </summary>
        /// <param name="sender">This. We do not use this.</param>
        /// <param name="e">It contains the Cancel property, that tell the program to be closed or not.</param>
        private void View_Closing(object sender, CancelEventArgs e)
        {
            _model.Pause();

            if (_viewModel.Saved == false && MessageBox.Show(_view, "Are you sure, you want to exit without save?", "Reversi", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.No)
            {
                e.Cancel = true; 
                _model.Unpause();
            }
        }

        #endregion

        #region ViewModel event handlers

        private void ViewModel_NewGame(object sender, EventArgs e)
        {
            _model.NewGame();
            _view.SizeToContent = SizeToContent.WidthAndHeight; //TODO: At 30x30 it is getting too big. Well at least it wont go out of the screenthat much. Maybe should set Maximum height.
        }

        private async void ViewModel_LoadGame(object sender, EventArgs e)
        {
            _model.Pause();

            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Title = "Loading Reversi game";
            openFileDialog.Filter = "Reversi game|*.reversi";

            if (openFileDialog.ShowDialog() == true)
            {
                try
                {
                    await _model.LoadGame(openFileDialog.FileName);
                    _viewModel.Saved = false;
                }
                catch (ReversiDataException ex)
                {
                    MessageBox.Show(_view, ex.ReversiMessage + System.Environment.NewLine + System.Environment.NewLine + ex.ReversiInfo, "Reversi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(_view, ex.Message, "Reversi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            _model.Unpause();
        }

        private async void ViewModel_SaveGame(object sender, EventArgs e)
        {
            _model.Pause();

            SaveFileDialog saveFileDialog = new SaveFileDialog();
            saveFileDialog.Title = "Saving Reversi game";
            saveFileDialog.Filter = "Reversi game|*.reversi";

            if (saveFileDialog.ShowDialog() == true)
            {
                try
                {
                    await _model.SaveGame(saveFileDialog.FileName);
                    _viewModel.Saved = true;
                }
                catch (ReversiDataException ex)
                {
                    MessageBox.Show(ex.ReversiMessage + System.Environment.NewLine + System.Environment.NewLine + ex.ReversiInfo, "Reversi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Reversi", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }

            _model.Unpause();
        }

        private void ViewModel_ExitApplication(object sender, EventArgs e)
        {
            _view.Close();
        }

        private void ViewModel_ReadRules(object sender, EventArgs e)
        {
            _model.Pause();

            MessageBox.Show(_view, "Always the white starts the game. If he can he chooses a put down location, only if he can not he passes. Then the black do the same then the white again, and so on ... ." + Environment.NewLine + "You have to straddle the enemy put downs to make a put down and to make them yours. You can do it in all directions. The game ends if no one can make a put down. The player with the more put downs win.", "Reversi", MessageBoxButton.OK, MessageBoxImage.None);

            _model.Unpause();
        }

        private void ViewModel_ReadAbout(object sender, EventArgs e)
        {
            _model.Pause();

            /* // It throws an exception, if opened the secound time.
            if (_aboutView == null)
            {
                _aboutView = new AboutMessage()
            }
            */

            _aboutView = new AboutMessage(); //TODO: Do I have to recreate it?
            _aboutView.ShowDialog();

            _model.Unpause();
        }

        #endregion

        #region Model event handlers

        /// <summary>
        /// Invoked by the model, when the game ended.
        /// </summary>
        /// <param name="sender">The model. We do not use this.</param>
        /// <param name="e">The data that helps us to write out the result of the game.</param>
        private void Model_SetGameEnded(object sender, ReversiSetGameEndedEventArgs e)
        {
            if (e.Player1Points > e.Player2Points)
            {
                MessageBox.Show(_view, "Player 1 won." + Environment.NewLine + "Player 1 points: " + e.Player1Points.ToString() + ", player 2 points: " + e.Player2Points.ToString() + ".", "Game Ended", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (e.Player1Points < e.Player2Points)
            {
                MessageBox.Show(_view, "Player 2 won." + Environment.NewLine + "Player 1 points: " + e.Player1Points.ToString() + ", player 2 points: " + e.Player2Points.ToString() + ".", "Game Ended", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show(_view, "It is a tie." + Environment.NewLine + "Player 1 points: " + e.Player1Points.ToString() + ", player 2 points: " + e.Player2Points.ToString() + ".", "Game Ended", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion
    }
}
