using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using System.Drawing;

namespace ClassLibraryRemotePC
{
    [Serializable]
    public class NetImage
    {
        public Bitmap image { set; get; }
        byte[] _data;
        public NetImage() { }
        public NetImage(Bitmap _image) 
        {
            image = _image;
        }
        public NetImage(byte[] data)
        {
            NetImage file = FromArray(data);
            image = file.image;
            //Data = file.Data;
        }
        public byte[] ToArray()
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                return stream.ToArray();
            }
        }
        public static NetImage FromArray(byte[] data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(data))
            {
                stream.Position = 0;
                return (NetImage)formatter.Deserialize(stream);
            }
        }
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
    }
}
