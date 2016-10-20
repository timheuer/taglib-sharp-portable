using System.IO;

namespace TagLib.Tests
{
    public class LocalFileAbstraction : File.IFileAbstraction
    {
        public LocalFileAbstraction(string path, bool openForWrite = false)
        {
            Name = Path.GetFullPath(path);
            _openForWrite = openForWrite;
        }

        public string Name { get; private set; }

        public Stream ReadStream
        {
            get
            {
                return System.IO.File.OpenRead(Name);
            }
        }

        public Stream WriteStream
        {
            get
            {
                return _openForWrite
                    ? System.IO.File.Open(Name, FileMode.Open, FileAccess.ReadWrite)
                    : ReadStream;
            }
        }

        public void CloseStream(Stream stream)
        {
            if (stream != null)
                stream.Dispose();
        }

        private readonly bool _openForWrite;
    }
}
