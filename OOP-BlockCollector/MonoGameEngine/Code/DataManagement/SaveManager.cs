using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace MonoGameEngine.DataManagement
{
    /// <summary>A class which provides access to input/output needed for managing saved game data (using the SaveFile class).</summary>
    public static class SaveManager
    {
        private const string FILE_PATH = "/Data/";
        private const string FILE_NAME = "SaveFile";
        private const string FILE_EXTENSION = ".dat";

        private static int _currentSaveIndex = -1;

        /// <summary>Property that provides easy access to the most recently loaded SaveFile.</summary>
        public static SaveFile CurrentSave { get; set; }

        /// <summary>
        /// Will save the passed Object to the allocated directory.
        /// </summary>
        /// <param name="saveFile">[Optional] The SaveFile which should be stored as a file on the machine's memory.</param>
        /// <param name="fileIndex">[Optional]An ID used to differentiate between different save files. Will use current save index if left empty.</param>
        /// <param name="append">[Optional] Should the save create a new file, or add to an existing one?</param>
        public static void Save(SaveFile saveFile = null, int? fileIndex = null, bool append = false)
        {
            saveFile ??= CurrentSave;
            if (saveFile == null)
                throw new Exception("Save operation failed; no save file has been previously loaded, nor has there been one provided as an argument to store.");

            if (fileIndex.HasValue)
                fileIndex = (int)fileIndex;
            else
                fileIndex = _currentSaveIndex;

            if(fileIndex >= 0)
            {
                CreateDirectory(FILE_PATH);
                using (Stream stream = File.Open(GenerateFilePath((int)fileIndex), append ? FileMode.Append : FileMode.Create))
                {
                    var binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(stream, saveFile);
                }
            }
            else
            {
                Console.Error.WriteLine("Invalid file index <" + fileIndex + ">. Save command aborted.");
            }
        }

        /// <summary>
        /// Will delete the save file at the given index, if it exists.
        /// </summary>
        /// <param name="fileIndex">The index of the file that should be deleted.</param>
        public static void Delete(int fileIndex)
        {
            if (fileIndex >= 0)
            {
                if (CheckFileExists(GenerateFilePath(fileIndex)))
                {
                    File.Delete(GenerateFilePath(fileIndex));
                }
            }
            else
            {
                Console.Error.WriteLine("Invalid file index <" + fileIndex + ">. Delete command aborted.");
            }
        }

        /// <summary>
        /// Will load the desired SaveFile, if it exists. 
        /// </summary>
        /// <param name="fileIndex">The index of the file that should be loaded.</param>
        /// <returns>The SaveFile object holding the restored data at the given index, if it exists. Otherwise, returns an empty SaveFile for that index.</returns>
        public static SaveFile Load(int fileIndex)
        {
            if(fileIndex >= 0)
            {
                _currentSaveIndex = fileIndex;

                if (CheckFileExists(GenerateFilePath(fileIndex)))
                {
                    using (Stream stream = File.Open(GenerateFilePath(fileIndex), FileMode.Open))
                    {
                        var binaryFormatter = new BinaryFormatter();
                        CurrentSave = (SaveFile)binaryFormatter.Deserialize(stream);
                        return CurrentSave;
                    }
                }
                CurrentSave = new SaveFile(fileIndex);
                return CurrentSave;
            }
            else
            {
                Console.Error.WriteLine("Invalid file index <" + fileIndex + ">. Load command aborted.");
            }

            return null;
        }

        /// <summary>
        /// Will create a location for files to be saved at the given directory, if one doesn't already exist.
        /// </summary>
        /// <param name="directory">The directory of the desired folder structure. Uses current directory of program as a root. </param>
        private static void CreateDirectory(string directory)
        {
            if(!Directory.Exists(Directory.GetCurrentDirectory() + directory))
            {
                if(Directory.CreateDirectory(Directory.GetCurrentDirectory() + directory).Exists)
                {
                    Debug.WriteLine("Directory created!");
                }
            }
        }

        /// <summary>
        /// Checks to see if a file with the given filename exists. The filename should include the directory the file belongs to.
        /// </summary>
        /// <param name="filename">The filename of the file that should be checked. </param>
        /// <returns>Returns 'true' if a file with the given name exists. Otherwise, returns 'false'.</returns>
        private static bool CheckFileExists(string filename)
        {
            return File.Exists(filename);
        }

        /// <summary>
        /// Creates a single String which describes the full directory filepath.
        /// </summary>
        /// <returns>A String object with the full directory filepath, assuming the main program folder as the root.</returns>
        private static string GenerateFileDirectoryString()
        {
            return Directory.GetCurrentDirectory() + FILE_PATH;
        }

        /// <summary>
        /// Creates a single String which describes the full directory filepath, plus the full file name.
        /// </summary>
        /// <param name="fileIndex">The specific save file ID that is desired. </param>
        /// <returns>A String object with the full directory filepath and file name, assuming the main program folder as the root.</returns>
        private static string GenerateFilePath(int fileIndex)
        {
            return GenerateFileDirectoryString() + FILE_NAME + fileIndex + FILE_EXTENSION;
        }
    }
}
