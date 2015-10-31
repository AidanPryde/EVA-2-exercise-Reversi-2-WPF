
using Reversi_WPF.Model;
using Reversi_WPF.Persistence;
using Reversi_WPF.View;
using Reversi_WPF.ViewModel;

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
            _viewModel.ExitGame += new EventHandler(ViewModel_ExitGame);

            _viewModel.ReadRules += new EventHandler(ViewModel_ReadRules);
            _viewModel.ReadAbout += new EventHandler(ViewModel_ReadAbout);

            _view = new MainWindow();
            _view.DataContext = _viewModel;
            _view.Closing += new CancelEventHandler(View_Closing);
            _view.Show();
        }

        #endregion

        #region View event handlers

        private void View_Closing(object sender, CancelEventArgs e)
        {

        }

        #endregion

        #region ViewModel event handlers

        private void ViewModel_NewGame(object sender, EventArgs e)
        {

        }

        private void ViewModel_LoadGame(object sender, EventArgs e)
        {

        }

        private void ViewModel_SaveGame(object sender, EventArgs e)
        {

        }

        private void ViewModel_ExitGame(object sender, EventArgs e)
        {

        }

        private void ViewModel_ReadRules(object sender, EventArgs e)
        {

        }

        private void ViewModel_ReadAbout(object sender, EventArgs e)
        {

        }

        #endregion

        #region Model event handlers

        private void Model_SetGameEnded(object sender, EventArgs e)
        {

        }

        #endregion
    }
}
