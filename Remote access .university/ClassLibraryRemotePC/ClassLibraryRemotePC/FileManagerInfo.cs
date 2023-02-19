using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Security;
using System.Runtime.Serialization.Formatters.Binary;

namespace ClassLibraryRemotePC
{
    /// <summary>
    /// Предоставляет механизмы для получения информации о логических дисках и объектах файловой системы.
    /// </summary>
    [Serializable]
    public class FileManagerInfo
    {
        private byte[] _data;
        /// <summary>
        /// Сериализированный объект FileManagerInfo.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return _data;
            }
            set
            {
                _data = value;
            }
        }
        /// <summary>
        /// Содержит информацию о логических дисках.
        /// </summary>
        public List<DriveInfo> drivesInfo { set; get; }
        /// <summary>
        /// Содержит информацио о объектах определенного каталога.
        /// </summary>
        public List<FileSystemInfo> fileSystemObjectsInfo { set; get; }

        public List<DateTime> CreationDate { set; get; }
        public List<DateTime> LastWriteTime { set; get; }
        public List<string> ObjName { set; get; }
        public List<string> FullName { set; get; }
        public List<FileAttributes> Attributes { set; get; }
        public List<long> LengthObj { set; get; }

        public string ThisDirPath  { set; get; }





        /// <summary>
        /// Инормация о всех логических дисках и объектах корневого каталога первого считанного логического диска.
        /// </summary>
        public FileManagerInfo()
        {
            drivesInfo = new List<DriveInfo>();
            fileSystemObjectsInfo = new List<FileSystemInfo>();
            GetDrives();
            if (drivesInfo.Count > 0)
            {
                GetObjectsInfo(drivesInfo[0].RootDirectory.FullName);
            }
        }
        /// <summary>
        /// Инормация о всех логических дисках и объектах указанного каталога.
        /// </summary>
        /// <param name="fullPath">Абсолютный путь каталога.</param>
        public FileManagerInfo(string fullPath)
        {
            drivesInfo = new List<DriveInfo>();
            fileSystemObjectsInfo = new List<FileSystemInfo>();
            GetDrives();
            GetObjectsInfo(fullPath);
        }
        /// <summary>
        /// Создает FileManagerInfo на основе массива байт.
        /// </summary>
        /// <param name="data">Массив байт с сериализованным объектом FileManagerInfo</param>
        public FileManagerInfo(byte[] data)
        {
            FileManagerInfo FS_Info = FromArray(data);

            fileSystemObjectsInfo = FS_Info.fileSystemObjectsInfo;
            LastWriteTime = FS_Info.LastWriteTime;
            Attributes = FS_Info.Attributes;
            LengthObj = FS_Info.LengthObj;
            drivesInfo = FS_Info.drivesInfo;
            ThisDirPath = FS_Info.ThisDirPath;
            Data = FS_Info.Data;
        }

        void GetDrives()
        {
            DriveInfo[] allDrives;
            try
            {
                allDrives = DriveInfo.GetDrives();
                if (allDrives.Length > 0)
                {
                    drivesInfo = allDrives.ToList();
                }
                else
                {
                    throw new Exception("Drives not found.");
                }
            }
            catch (IOException ex)
            {
                throw new Exception("The drive error or drive was not ready.", ex);
            }
            catch(UnauthorizedAccessException ex)
            {
                throw new Exception("The caller does not have the required permission.", ex);
            }
        }
        void GetObjectsInfo(string path)
        {
            DirectoryInfo dir;
            FileSystemInfo[] infos;
            try
            {
                dir = new DirectoryInfo(path);
                this.ThisDirPath = path;
            }
            catch(ArgumentNullException ex)
            {
                throw new Exception("Path is null.", ex);
            }
            catch (SecurityException ex)
            {
                throw new Exception("The caller does not have the required permission.", ex);
            }
            catch (ArgumentException ex)
            {
                throw new Exception("Path contains invalid characters such as \", <,>, or |.", ex);
            }
            catch (PathTooLongException ex)
            {
                throw new Exception("The specified path, file name, or both exceed the maximum length specified by the system.", ex);
            }
            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException("The directory does not exist.");
            }
            else
            {
                infos = dir.GetFileSystemInfos();
                fileSystemObjectsInfo = infos.ToList();
                this.LastWriteTime = new List<DateTime>();
                this.Attributes = new List<FileAttributes>();
                this.LengthObj = new List<long>();
                foreach (FileSystemInfo fs in this.fileSystemObjectsInfo)
                {
                    this.LastWriteTime.Add(fs.LastWriteTime);
                    this.Attributes.Add(fs.Attributes);
                    try
                    {
                        FileInfo fileInfo = new FileInfo(fs.FullName);
                        this.LengthObj.Add(fileInfo.Length);
                    }
                    catch
                    {
                        this.LengthObj.Add(-1);
                    }
                }
            }
        }

        /// <summary>
        /// Сериализирует объект FileManagerInfo в массив байт.
        /// </summary>
        /// <returns>Массив байт.</returns>
        public byte[] ToArray()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                return stream.ToArray();
            }
        }
        /// <summary>
        /// Десериализирует объект FileManagerInfo из массива байт.
        /// </summary>
        /// <param name="data">Массив байт.</param>
        /// <returns>Объект FileManagerInfo полученный в результате десериализации.</returns>
        public static FileManagerInfo FromArray(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(data))
            {
                stream.Position = 0;
                return (FileManagerInfo)formatter.Deserialize(stream);
            }
        }

    }
}
