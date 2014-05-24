using System.IO;

namespace TagLib.Tests
{
    public class LocalFileAbstraction : File.IFileAbstraction
    {
        public LocalFileAbstraction(string path, bool openForWrite = false)
        {
            Name = Path.GetFileName(path);
            var fileStream = openForWrite ? System.IO.File.Open(path, FileMode.Open, FileAccess.ReadWrite) : System.IO.File.OpenRead(path);
            ReadStream = WriteStream = fileStream;
        }

        public string Name { get; private set; }

        public Stream ReadStream { get; private set; }

        public Stream WriteStream { get; private set; }

        public void CloseStream(Stream stream)
        {
            if (stream != null)
            {
                stream.Close();
            }
        }
    }
}
