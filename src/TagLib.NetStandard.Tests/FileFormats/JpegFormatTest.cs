using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using TagLib.IFD;
using TagLib.IFD.Entries;
using TagLib.IFD.Tags;
using TagLib.Jpeg;
using TagLib.Xmp;

namespace TagLib.Tests.FileFormats
{
    [TestClass]
    public class JpegFormatTest
    {
        private const string SAMPLE_FILE = "samples/sample.jpg";
        private Image.File _file;

        private const TagTypes CONTAINED_TYPES = TagTypes.JpegComment | TagTypes.TiffIFD | TagTypes.XMP;

        [TestInitialize]
        public void Init()
        {
            _file = File.Create(new LocalFileAbstraction(SAMPLE_FILE)) as Image.File;
        }

        [TestMethod]
        public void TestJpegRead()
        {
            Assert.IsTrue(_file is Jpeg.File);

            Assert.AreEqual(CONTAINED_TYPES, _file.TagTypes);
            Assert.AreEqual(CONTAINED_TYPES, _file.TagTypesOnDisk);

            Assert.IsNotNull(_file.Properties, "properties");
            Assert.AreEqual(7, _file.Properties.PhotoHeight);
            Assert.AreEqual(10, _file.Properties.PhotoWidth);
            Assert.AreEqual(90, _file.Properties.PhotoQuality);

            JpegCommentTag tag = _file.GetTag(TagTypes.JpegComment) as JpegCommentTag;
            Assert.IsFalse(tag == null);
            Assert.AreEqual("Test Comment", tag.Value);
        }

        [TestMethod]
        public void TestExif()
        {
            var tag = _file.GetTag(TagTypes.TiffIFD) as IFDTag;
            Assert.IsFalse(tag == null);

            var exifIfd = tag.Structure.GetEntry(0, IFDEntryTag.ExifIFD) as SubIFDEntry;
            Assert.IsFalse(exifIfd == null);
            var exifTag = exifIfd.Structure;

            {
                var entry = exifTag.GetEntry(0, (ushort)ExifEntryTag.ExposureTime) as RationalIFDEntry;
                Assert.IsFalse(entry == null);
                Assert.AreEqual(0.008, entry.Value);
            }
            {
                var entry = exifTag.GetEntry(0, (ushort)ExifEntryTag.FNumber) as RationalIFDEntry;
                Assert.IsFalse(entry == null);
                Assert.AreEqual(3.2, entry.Value);
            }
            {
                var entry = exifTag.GetEntry(0, (ushort)ExifEntryTag.ISOSpeedRatings) as ShortIFDEntry;
                Assert.IsFalse(entry == null);
                Assert.AreEqual(100, entry.Value);
            }
        }

        [TestMethod]
        public void TestXmp()
        {
            XmpTag tag = _file.GetTag(TagTypes.XMP) as XmpTag;
            Assert.IsFalse(tag == null);

            TestBagNode(tag, XmpTag.DC_NS, "subject", new[] { "keyword1", "keyword2", "keyword3" });
            TestAltNode(tag, XmpTag.DC_NS, "description", new[] { "Sample Image" });
        }

        [TestMethod]
        public void TestConstructor1()
        {
            var file = new Jpeg.File(new LocalFileAbstraction(SAMPLE_FILE), ReadStyle.None);
            Assert.IsNotNull(file.ImageTag, "ImageTag");
            Assert.AreEqual(CONTAINED_TYPES, file.TagTypes);

            Assert.IsNotNull(file.Properties, "properties");
        }

        [TestMethod]
        public void TestConstructor2()
        {
            var file = new Jpeg.File(new LocalFileAbstraction(SAMPLE_FILE), ReadStyle.None);
            Assert.IsNotNull(file.ImageTag, "ImageTag");
            Assert.AreEqual(CONTAINED_TYPES, file.TagTypes);

            Assert.IsNull(file.Properties, "properties");
        }

        [TestMethod]
        public void TestConstructor3()
        {
            var file = new Jpeg.File(new LocalFileAbstraction(SAMPLE_FILE), ReadStyle.None);
            Assert.IsNotNull(file.ImageTag, "ImageTag");
            Assert.AreEqual(CONTAINED_TYPES, file.TagTypes);

            Assert.IsNull(file.Properties, "properties");
        }

        private void TestBagNode(XmpTag tag, string ns, string name, string[] values)
        {
            var node = tag.FindNode(ns, name);
            Assert.IsFalse(node == null);
            Assert.AreEqual(XmpNodeType.Bag, node.Type);
            Assert.AreEqual(values.Length, node.Children.Count);

            int i = 0;
            foreach (var childNode in node.Children)
            {
                Assert.AreEqual(values[i], childNode.Value);
                Assert.AreEqual(0, childNode.Children.Count);
                i++;
            }
        }

        private void TestAltNode(XmpTag tag, string ns, string name, string[] values)
        {
            var node = tag.FindNode(ns, name);
            Assert.IsFalse(node == null);
            Assert.AreEqual(XmpNodeType.Alt, node.Type);
            Assert.AreEqual(values.Length, node.Children.Count);

            int i = 0;
            foreach (var childNode in node.Children)
            {
                Assert.AreEqual(values[i], childNode.Value);
                Assert.AreEqual(0, childNode.Children.Count);
                i++;
            }
        }
    }
}
