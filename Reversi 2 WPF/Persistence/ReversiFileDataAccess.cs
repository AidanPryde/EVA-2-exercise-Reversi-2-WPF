
using System;
using System.IO;
using System.Threading.Tasks;

namespace Reversi_WPF.Persistence
{
    /// <summary>
    /// The type of the Reversi file manager.
    /// </summary>
    public class ReversiFileDataAccess : IReversiDataAccess
    {

        #region Fields

        private readonly Int32[] _supportedGameTableSizesArray;

        #endregion

        #region Properties

        public Int32[] SupportedGameTableSizesArray
        {
            get
            {
                return _supportedGameTableSizesArray;
            }
        }

        #endregion

        #region Constructors

        public ReversiFileDataAccess(Int32[] supportedGameTableSizesArray = null)
        {
            if (supportedGameTableSizesArray == null)
            {
                _supportedGameTableSizesArray = new Int32[] { 10 };
            }
            else
            {
                _supportedGameTableSizesArray = supportedGameTableSizesArray;
            }
        }

        #endregion

        #region Public methodes

        /// <summary>
        /// Loading file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <returns>All the data to recover the game state. The game table size, the put downs and the players game times.</returns>
        public async Task<ReversiGameDescriptiveData> Load(String path)
        {
            using (StreamReader reader = new StreamReader(path)) // opening file
            {
                // Read the first line of the file, then split it bye one space.
                String line = await reader.ReadLineAsync();
                char[] separatingChars = { ' ' };
                String[] numbers = line.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);

                // Setup fields with the processed data. The ordering is set by the file format.
                // Throw IndexOutOfRangeException if some data missing or FormatException if there are spaces.
                Int32 tableSize = Int32.Parse(numbers[0]);
                Int32 player1Time = Int32.Parse(numbers[1]);
                Int32 player2Time = Int32.Parse(numbers[2]);
                Int32 putDownsSize = Int32.Parse(numbers[3]);

                Boolean found = false;
                for (Int32 i = 0; i < _supportedGameTableSizesArray.GetLength(0) && !found; ++i)
                {
                    if (tableSize == _supportedGameTableSizesArray[i])
                    {
                        found = true;
                    }
                }

                if (!found)
                {
                    String supportedGameTableSizesString = "";
                    for (Int32 i = 0; i < _supportedGameTableSizesArray.GetLength(0); ++i)
                    {
                        supportedGameTableSizesString += _supportedGameTableSizesArray[i].ToString() + ", ";
                    }

                    throw new ReversiDataException("Error while trying to load file: " + path + ".", "The read table size ( " + tableSize.ToString() + " ) is not supported ( " + supportedGameTableSizesString + ").");
                }

                if (player1Time < 0 || player2Time < 0)
                {
                    throw new ReversiDataException("Error while trying to load file: " + path + ".", "The read player 1 time ( " + player1Time.ToString() + " ) or/and player 2 time ( " + player2Time.ToString() + " ) was/were negative.");
                }

                if (putDownsSize % 2 == 1 || (tableSize * tableSize * 4) < putDownsSize)
                {
                    throw new ReversiDataException("Error while trying to load file: " + path + ".", "The read put down size ( " + putDownsSize.ToString() + " ) was odd or bigger then the passible with the given table size ( " + tableSize.ToString() + " )");
                }

                // Creating the game descriptive data class.
                ReversiGameDescriptiveData data = new ReversiGameDescriptiveData(tableSize, player1Time, player2Time, putDownsSize);

                // Read a line of the file, then split it bye one space.
                if (putDownsSize > 0)
                {
                    line = await reader.ReadLineAsync();
                    numbers = line.Split(separatingChars, StringSplitOptions.RemoveEmptyEntries);

                    // Setup values of the putDown array.
                    for (Int32 i = 0; i < putDownsSize; ++i)
                    {
                        data[i] = Int32.Parse(numbers[i]);
                    }
                }

                return data;
            }
        }

        /// <summary>
        /// Saving file.
        /// </summary>
        /// <param name="path">File path.</param>
        /// <param name="data">All the data to recover the game state. The game table size, the put downs and the players game times.</param>
        public async Task Save(String path, ReversiGameDescriptiveData data)
        {
            using (StreamWriter writer = new StreamWriter(path)) // opening file
            {
                // Writing the fist line with the size if the table, with the players played times and with the coordinats count.
                writer.Write(data.TableSize.ToString() + " " + data.Player1Time.ToString() + " " + data.Player2Time.ToString() + " " + data.PutDownsSize.ToString());
                await writer.WriteLineAsync();

                // Writing the second line with the coordinates.
                for (Int32 i = 0; i < data.PutDownsSize - 1; ++i)
                {
                    await writer.WriteAsync(data[i].ToString() + " ");
                }

                if (data.PutDownsSize != 0)
                {
                    await writer.WriteAsync(data[data.PutDownsSize - 1].ToString());
                }
                    
            }
        }

        #endregion

    }
}
