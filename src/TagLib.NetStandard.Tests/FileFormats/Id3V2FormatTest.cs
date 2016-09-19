using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TagLib.Tests.FileFormats
{
    [TestClass]
    public class Id3V2FormatTest : IFormatTest
    {
        private const string SAMPLE_FILE = "samples/sample_v2_only.mp3";
        private const string CORRUPT_FILE = "samples/corrupt/null_title_v2.mp3";
        private const string TMP_FILE = "samples/tmpwrite_v2_only.mp3";
        private const string EXT_HEADER_FILE = "samples/sample_v2_3_ext_header.mp3";
        private File _file;

        [TestInitialize]
        public void Init()
        {
            _file = File.Create(new LocalFileAbstraction(SAMPLE_FILE));
        }

        [TestMethod]
        public void ReadAudioProperties()
        {
            Assert.AreEqual(44100, _file.Properties.AudioSampleRate);
            Assert.AreEqual(1, _file.Properties.Duration.Seconds);
        }

        [TestMethod]
        public void TestExtendedHeaderSize()
        {
            // bgo#604488
            var file = File.Create(new LocalFileAbstraction(EXT_HEADER_FILE));
            Assert.AreEqual("Title v2", file.Tag.Title);
        }

        [TestMethod] // http://bugzilla.gnome.org/show_bug.cgi?id=558123
        public void TestTruncateOnNull()
        {
            if (System.IO.File.Exists(TMP_FILE))
            {
                System.IO.File.Delete(TMP_FILE);
            }

            System.IO.File.Copy(CORRUPT_FILE, TMP_FILE);
            File tmp = File.Create(new LocalFileAbstraction(TMP_FILE));

            Assert.AreEqual("T", tmp.Tag.Title);
        }

        [TestMethod]
        public void ReadTags()
        {
            Assert.AreEqual("MP3 album", _file.Tag.Album);
            Assert.AreEqual("MP3 artist", _file.Tag.FirstPerformer);
            Assert.AreEqual("MP3 comment", _file.Tag.Comment);
            Assert.AreEqual("Acid Punk", _file.Tag.FirstGenre);
            Assert.AreEqual("MP3 title", _file.Tag.Title);
            Assert.AreEqual((uint)6, _file.Tag.Track);
            Assert.AreEqual((uint)7, _file.Tag.TrackCount);
            Assert.AreEqual((uint)1234, _file.Tag.Year);
        }

        [TestMethod]
        public void TestCorruptionResistance()
        {
            
        }
    }
}
