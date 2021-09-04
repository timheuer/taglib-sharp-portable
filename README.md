TagLib#.Portable
=====================

[![Build](https://github.com/timheuer/taglib-sharp-portable/actions/workflows/build_deploy.yaml/badge.svg?branch=main)](https://github.com/timheuer/taglib-sharp-portable/actions/workflows/build_deploy.yaml)

**IMPORTANT - PLEASE READ**
==============
The mono/taglib-sharp project has since updated to .NET Standard and is more maintained than this one. The history of this was originally bringing a *portable* version across Silverlight, Framework, and other targets where Mono wasn't doing that at the time. It appears that project is being maintained now and you should strongly evaluate whether that one is better for you...it likely is 😉



Project Information
=
This is a fork of the popular TagLib# project.  

> TagLib is a library for reading and editing the meta-data of several popular audio formats. Currently it supports both ID3v1 and ID3v2 for MP3 files, Ogg Vorbis comments and ID3 tags and Vorbis comments in FLAC, MPC, Speex, WavPack TrueAudio, WAV, AIFF, MP4 and ASF files.

This is re-implemented as a .NET Standard (1.6) library that can be used by several .NET platforms. See https://docs.microsoft.com/en-us/dotnet/standard/net-standard for compatibility.

This is a work in progress and there are still some things needed to be done (see [issues list](https://github.com/timheuer/taglib-sharp-portable/issues)).

Key Changes
=
Perhaps the biggest change is the removal of `LocalFileAbstraction`, which in TagLib# was the default when reading a file into the library.  This was done to maximize portability without trying to do some fancy hoop-jumping to isolated a portable storage mechanism.  A `StreamFileAbstraction` was added and could easily be used as the primary mechanism in the various `File.Create` methods used to read in a file.

Installation
=
If you clone and build from source, from a VS command prompt just type `build`.  You can also get the most current version from [NuGet directly](https://www.nuget.org/packages/TagLib.Portable/).

Sample Usage
=
To read in an MP3 file (using WinRT as an example):

    // assume you've got to the point where you have a StorageFile 
    // via a file picker or something similar
    
    var fileStream = await (StorageFile)file.OpenStreamForReadAsync();

	var tagFile = TagLib.File.Create(new StreamFileAbstraction(file.Name,
					 fileStream, fileStream);

	var tags = tagFile.GetTag(TagTypes.Id3v2);

	Debug.WriteLine(tags.Title);

There are other ways you can do this as well and this is just a simplest example.

History
=
For some brief history, please read [http://timheuer.com/blog/archive/2014/05/23/porting-taglib-sharp-to-portable-class-library.aspx](http://timheuer.com/blog/archive/2014/05/23/porting-taglib-sharp-to-portable-class-library.aspx).