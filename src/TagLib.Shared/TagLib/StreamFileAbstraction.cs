//
// StreamFileAbstraction.cs:
//
// Author:
//   Tim Heuer (tim@timheuer.com)
//
// Copyright (C) 2014 Tim Heuer
//
// This library is free software; you can redistribute it and/or modify
// it  under the terms of the The MIT License (MIT).
//
// This library is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  
// See The MIT License (MIT) for more details.
//
// You should have received a copy of The MIT License (MIT)
// along with this library.
//

using System;
using System.IO;

namespace TagLib
{
    public class StreamFileAbstraction : File.IFileAbstraction
    {
        /// <summary>
        /// Initializes a StreamFileAbstaction with a Byte[] of data for the media
        /// Example usage:
        /// 
        ///                var abs = new StreamFileAbstraction(filename, songData);
        ///                using (var file = File.Create(abs))
        ///                {
        ///                    file.Tag.Album = Album;
        ///                    file.Tag.Title = SongTitle;
        ///                    file.Tag.AlbumArtists = new[] {Artist};
        ///                    file.Tag.AmazonId = Song.AmazonTrackId;
        ///                    file.Save();
        ///                }
        ///                StorageFile sf = await folder.CreateFileAsync(filename, CreationCollisionOption.FailIfExists);
        ///                sf.WriteAllBytes(abs.TaggedMediaData); 
        /// 
        /// </summary>
        /// <param name="name"> Name of the media file</param>
        /// <param name="data"> Byte Array of the media source</param>
        public StreamFileAbstraction(string name, byte[] data)
        {
            ReadStream = new MemoryStream();
            ReadStream.Write(data, 0, data.Length);
            WriteStream = ReadStream;
            Name = name;
        }

        public StreamFileAbstraction(string name, MemoryStream readStream)
        {
            // TODO: Fix deadlock when setting an actual writable Stream
            WriteStream = readStream;
            ReadStream = readStream;
            Name = name;
        }

        public string Name {
            get; private set;
        }

        public MemoryStream ReadStream {
            get; private set;
        }

        public MemoryStream WriteStream {
            get; private set;
        }

        public void CloseStream(MemoryStream stream)
        {
            stream.Dispose();
        }

        /// <summary>
        /// Byte Array representing the new media file that has been tagged.
        /// Make sure you call Save before accessing.
        /// </summary>
        public byte[] TaggedMediaData => WriteStream.ReadToEnd();
    }

    public static class StreamHelper
    {
        public static byte[] ReadToEnd(this System.IO.Stream stream)
        {
            long originalPosition = 0;

            if (stream.CanSeek)
            {
                originalPosition = stream.Position;
                stream.Position = 0;
            }

            try
            {
                byte[] readBuffer = new byte[4096];

                int totalBytesRead = 0;
                int bytesRead;

                while ((bytesRead = stream.Read(readBuffer, totalBytesRead, readBuffer.Length - totalBytesRead)) > 0)
                {
                    totalBytesRead += bytesRead;

                    if (totalBytesRead != readBuffer.Length)
                        continue;
                    int nextByte = stream.ReadByte();
                    if (nextByte == -1)
                        continue;
                    byte[] temp = new byte[readBuffer.Length * 2];
                    Buffer.BlockCopy(readBuffer, 0, temp, 0, readBuffer.Length);
                    Buffer.SetByte(temp, totalBytesRead, (byte)nextByte);
                    readBuffer = temp;
                    totalBytesRead++;
                }

                byte[] buffer = readBuffer;
                if (readBuffer.Length == totalBytesRead)
                    return buffer;
                buffer = new byte[totalBytesRead];
                Buffer.BlockCopy(readBuffer, 0, buffer, 0, totalBytesRead);
                return buffer;
            }
            finally
            {
                if (stream.CanSeek)
                {
                    stream.Position = originalPosition;
                }
            }
        }
    }
}
