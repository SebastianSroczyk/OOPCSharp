using System;
using System.Collections.Generic;
using System.Text;

namespace MonoGameEngine.DataManagement
{
    /// <summary>A class that can contain any necessary saved data for a game project. Generally used in conjunction with SaveManager.</summary>
    [Serializable]
    public sealed class SaveFile
    {
        /// <summary>The data structure which holds all of the requested data. Set up as a key/value pair, where the value can be of varying data types.</summary>
        private readonly Dictionary<string, object> _savedData;

        /// <summary>
        /// Internal constructor method.
        /// </summary>
        /// <param name="fileIndex">Used to identify different saved files.</param>
        internal SaveFile(int fileIndex)
        {
            if(fileIndex < 0)
            {
                Console.Error.WriteLine("Invalid file index <" + fileIndex + ">. SaveFile not created.");
            }
            else
            {
                _savedData = new Dictionary<string, object>
                {
                    { "fileIndex", fileIndex }
                };
            }
            
        }

        /// <summary>
        /// Adds data to this SaveFile, using a Key/Value Pair.
        /// <br/>If the key already exists, the held value at the given key will be overwritten instead.
        /// </summary>
        /// <param name="key">The identifying key to this piece of data.</param>
        /// <param name="value">The data value you wish to store. This can be any primitive data type.</param>
        public void AddData(string key, object value)
        {
            // Only adds a data key if one doesn't already exist. Otherwise, uses the key as an index to modify the existing associated value.
            if (_savedData.ContainsKey(key))
                _savedData[key] = value;
            else
                _savedData.Add(key, value);
        }

        /// <summary>
        /// Removes data from this SaveFile with a given key, if any exists.
        /// </summary>
        /// <param name="key">The identifying key to the piece of data that should be removed.</param>
        public void RemoveData(string key)
        {
            if (_savedData.ContainsKey(key))
                _savedData.Remove(key);
            else
                Console.WriteLine("No data at key <" + key + "> exists. Cannot remove data from SaveFile.");
        }

        /// <summary>
        /// Returns a piece of data with the associated given key, if that key exists within this SaveFile.
        /// </summary>
        /// <typeparam name="T">The data type of the data that has been requested.</typeparam>
        /// <param name="key">The identifying key to the desired piece of data.</param>
        /// <returns>The piece of data for the given key (if it exists), cast as the requested 'T' data type. Otherwise, returns appropriately-typed 'empty' data.</returns>
        public T GetData<T>(string key)
        {
            if (_savedData.TryGetValue(key, out object data))
            {
                return (T)data;
            }
            else
            {
                Console.Error.WriteLine("Save file does not contain data for the requested key <" + key + ">. Returning empty data instead.");
                if(typeof(T) == typeof(string))
                {
                    data = "";
                    return (T)data;
                }
                return default;
            }
        }

        /// <summary>
        /// Getter method used to access the file index of this SaveFile.
        /// </summary>
        /// <returns>The numerical index for this file.</returns>
        public int GetIndex()
        {
            return (int)_savedData["fileIndex"];
        }

        /// <summary>
        /// Converts this SaveFile object into a String, which can be written to an external text file.
        /// </summary>
        /// <returns>A String representation of the data held within this SaveFile object.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append(_savedData.ToString()).Append("\n");

            return stringBuilder.ToString();
        }
    }
}
