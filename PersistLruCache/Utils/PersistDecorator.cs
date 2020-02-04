using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Timers;

namespace PersistLruCache.Utils
{
    // Decorator for adding persist ability to any class, need to supply T as DataSource class for the decorator
    public abstract class PersistDecorator<T> : IPersist<T> where T : class
    {
        #region Properties
        public Timer PersistTimer { get; set; }
        public string PersistPrefix { get; set; }
        public string StoreDirectory { get; set; }
        public T DataSource { get; set; }
        #endregion

        #region Methods
        // the first initialization of the Datasource from potentially pre existing serialized file
        public void InitializeFromFile()
        {
            try
            {
                string persistFilePath = $"{StoreDirectory}\\{PersistPrefix}_persist";

                if (File.Exists(persistFilePath))
                {
                    using (FileStream fs = new FileStream(persistFilePath, FileMode.Open))
                    {
                        IFormatter bf = new BinaryFormatter();
                        var cache = (T)bf.Deserialize(fs);

                        DataSource = cache;

                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //can be used for logging the error or rethrowing the exception
            }
        }

        // The persistence action that will write the DataSource to a file when the elapsed event of the Timer occures
        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            try
            {
                if (DataSource != null)
                {
                    string persistFilePath = $"{StoreDirectory}\\{PersistPrefix}_persist";

                    if (File.Exists(persistFilePath))
                    {
                        File.Delete(persistFilePath);
                    }

                    using (FileStream fs = new FileStream(persistFilePath, FileMode.Create))
                    {
                        IFormatter bf = new BinaryFormatter();
                        bf.Serialize(fs, DataSource);
                        fs.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                //can be used for logging the error or rethrowing the exception
            }
        } 
        #endregion
    }
}
