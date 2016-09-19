using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace TagLib.Tests.FileFormats
{
    [TestClass]
    public class AsfFormatTest : IFormatTest
    {
        private const string SAMPLE_FILE = "samples/sample.wma";
        private const string TMP_FILE = "samples/tmpwrite.wma";
        private File _file;

        [TestInitialize]
        public void Init()
        {
            _file = File.Create(new LocalFileAbstraction(SAMPLE_FILE));
        }

        [TestMethod]
        public void ReadAudioProperties()
        {
            StandardTests.ReadAudioProperties(_file);
        }

        [TestMethod]
        public void ReadTags()
        {
            Assert.AreEqual("WMA album", _file.Tag.Album);
            Assert.AreEqual("Dan Drake", _file.Tag.FirstAlbumArtist);
            Assert.AreEqual("WMA artist", _file.Tag.FirstPerformer);
            Assert.AreEqual("WMA comment", _file.Tag.Comment);
            Assert.AreEqual("Brit Pop", _file.Tag.FirstGenre);
            Assert.AreEqual("WMA title", _file.Tag.Title);
            Assert.AreEqual((uint)5, _file.Tag.Track);
            Assert.AreEqual((uint)2005, _file.Tag.Year);
            Assert.AreEqual(1, _file.Tag.Pictures.Count(), "Embedded Album Art Found");
        }

        [TestMethod]
        public void WriteStandardTags()
        {
            StandardTests.WriteStandardTags (SAMPLE_FILE, TMP_FILE);
        }

        [TestMethod]
        public void TestCorruptionResistance()
        {
            StandardTests.TestCorruptionResistance ("samples/corrupt/a.wma");
        }
    }
}
