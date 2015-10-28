
using System;

namespace Reversi.Persistence
{
    /// <summary>
    /// The type of the Reversi data access exception.
    /// </summary>
    public class ReversiDataException : Exception
    {

        #region Fields

        private String _message;

        #endregion

        #region Properties

        override public String Message
        {
            get
            {
                return _message;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Create Reversi data access exception instance.
        /// </summary>
        public ReversiDataException(String source, String message)
        {
            this.Source = source;
            _message = message;
        }

        #endregion

    }
}
