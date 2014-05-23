using System.IO;

namespace TagLib
{
    public class StreamFileAbstraction : File.IFileAbstraction
    {
        public StreamFileAbstraction(string name, Stream readStream, Stream writeStream)
        {
            WriteStream = readStream;
            ReadStream = readStream;
            Name = name;
        }

        public string Name { get; private set; }

        public System.IO.Stream ReadStream { get; private set; }

        public System.IO.Stream WriteStream { get; private set; }

        public void CloseStream(System.IO.Stream stream)
        {
#if PORTABLE
            stream.Flush();
#else
            stream.Close();
#endif
        }
    }
}
