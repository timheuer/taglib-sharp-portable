//
// Item.cs: Provides a representation of an APEv2 tag item which can be read
// from and written to disk.
//
// Author:
//   Brian Nickel (brian.nickel@gmail.com)
//
// Original Source:
//   apeitem.cpp from TagLib
//
// Copyright (C) 2005-2007 Brian Nickel
// Copyright (C) 2004 by Allan Sandfeld Jensen (Original Implementation)
// 
// This library is free software; you can redistribute it and/or modify
// it  under the terms of the GNU Lesser General Public License version
// 2.1 as published by the Free Software Foundation.
//
// This library is distributed in the hope that it will be useful, but
// WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, write to the Free Software
// Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307
// USA
//

using System;

namespace TagLib.Ape {
	/// <summary>
	///    Indicates the type of data stored in a <see cref="Item" />
	///    object.
	/// </summary>
	public enum ItemType {
		/// <summary>
		///    The item contains Unicode text.
		/// </summary>
		Text = 0,
		
		/// <summary>
		///    The item contains binary data.
		/// </summary>
		Binary = 1,
		
		/// <summary>
		///    The item contains a locator (file path/URL) for external
		///    information.
		/// </summary>
		Locator = 2
	}
	
	/// <summary>
	///    This class provides a representation of an APEv2 tag item which
	///    can be read from and written to disk.
	/// </summary>
	public class Item 
	{
		#region Private Fields
		
		/// <summary>
		///    Contains the type of data stored in the item.
		/// </summary>
		private ItemType _type = ItemType.Text;
		
		/// <summary>
		///    Contains the item key.
		/// </summary>
		private string _key;
		
		/// <summary>
		///    Contains the item value.
		/// </summary>
		private ReadOnlyByteVector _data;
		
		/// <summary>
		///    Contains the item text.
		/// </summary>
		private string [] _text;
		
		/// <summary>
		///    Indicates whether or not the item is read only.
		/// </summary>
		private bool _readOnly;
		
		/// <summary>
		///    Contains the size of the item on disk.
		/// </summary>
		private int _sizeOnDisk;
		
		#endregion
		
		
		
		#region Constructors
		
		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="Item" />  by reading in a raw APEv2 item.
		/// </summary>
		/// <param name="data">
		///    A <see cref="ByteVector" /> object containing the item to
		///    read.
		/// </param>
		/// <param name="offset">
		///    A <see cref="int" /> value specifying the offset in
		///    <paramref name="data" /> at which the item data begins.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="data" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///    <paramref name="offset" /> is less than zero.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///    A complete item could not be read.
		/// </exception>
		public Item (ByteVector data, int offset)
		{
			if (data == null)
				throw new ArgumentNullException ("data");
			
			Parse (data, offset);
		}
		
		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="Item" /> with a specified key and value.
		/// </summary>
		/// <param name="key">
		///    A <see cref="string" /> object containing the key to use
		///    for the current instance.
		/// </param>
		/// <param name="value">
		///    A <see cref="string" /> object containing the value to
		///    store in the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="key" /> or <paramref name="value" /> is
		///    <see langword="null" />.
		/// </exception>
		public Item (string key, string value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			
			if (value == null)
				throw new ArgumentNullException ("value");
			
			_key = key;
			_text = new[] {value};
		}
		
		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="Item" /> with a specified key and collection of
		///    values.
		/// </summary>
		/// <param name="key">
		///    A <see cref="string" /> object containing the key to use
		///    for the current instance.
		/// </param>
		/// <param name="value">
		///    A <see cref="string[]" /> containing the values to store
		///    in the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="key" /> or <paramref name="value" /> is
		///    <see langword="null" />.
		/// </exception>
		public Item (string key, params string [] value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			
			if (value == null)
				throw new ArgumentNullException ("value");
			
			_key = key;
			_text = (string[]) value.Clone ();
		}
		
		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="Item" /> with a specified key and collection of
		///    values.
		/// </summary>
		/// <param name="key">
		///    A <see cref="string" /> object containing the key to use
		///    for the current instance.
		/// </param>
		/// <param name="value">
		///    A <see cref="StringCollection" /> object containing the
		///    values to store in the new instance.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="key" /> or <paramref name="value" /> is
		///    <see langword="null" />.
		/// </exception>
		/// <seealso cref="Item(string,string[])" />
		[Obsolete("Use Item(string,string[])")]
		public Item (string key, StringCollection value)
		{
			if (key == null)
				throw new ArgumentNullException ("key");
			
			if (value == null)
				throw new ArgumentNullException ("value");
			
			_key = key;
			_text = value.ToArray ();
		}
		
		/// <summary>
		///    Constructs and initializes a new instance of <see
		///    cref="Item" /> with a specified key and raw data.
		/// </summary>
		/// <param name="key">
		///    A <see cref="string" /> object containing the key to use
		///    for the current instance.
		/// </param>
		/// <param name="value">
		///    A <see cref="StringCollection" /> object containing the
		///    values to store in the new instance.
		/// </param>
		/// <remarks>
		///    This constructor automatically marks the new instance as
		///    <see cref="ItemType.Binary" />.
		/// </remarks>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="key" /> or <paramref name="value" /> is
		///    <see langword="null" />.
		/// </exception>
		/// <seealso cref="Item(string,string[])" />
		public Item (string key, ByteVector value)
		{
			_key = key;
			_type = ItemType.Binary;
			
			_data = value as ReadOnlyByteVector;
			if (_data == null)
				_data = new ReadOnlyByteVector (value);
		}
		
		private Item (Item item)
		{
			_type = item._type;
			_key = item._key;
			if (item._data != null)
				_data = new ReadOnlyByteVector (item._data);
			if (item._text != null)
				_text = (string[]) item._text.Clone ();
			_readOnly = item._readOnly;
			_sizeOnDisk = item._sizeOnDisk;
		}
		
		#endregion
		
		
		
		#region Public Properties
		
		/// <summary>
		///    Gets the key used to identify the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="string" /> object containing the key used to
		///    identify the current instance.
		/// </value>
		/// <remarks>
		///    This value is used for specifying the contents of the
		///    item in a common and consistant fashion. For example,
		///    <c>"TITLE"</c> specifies that the item contains the title
		///    of the track.
		/// </remarks>
		public string Key {
			get {return _key;}
		}
		
		/// <summary>
		///    Gets the binary value stored in the current instance.
		/// </summary>
		/// <value>
		///    A <see cref="ByteVector" /> object containing the binary
		///    value stored in the current instance, or <see
		///    langword="null" /> if the item contains text.
		/// </value>
		public ByteVector Value {
			get {return (_type == ItemType.Binary) ? _data : null;}
		}
		
		/// <summary>
		///    Gets the size of the current instance as it last appeared
		///    on disk.
		/// </summary>
		/// <value>
		///    A <see cref="int" /> value containing the size of the
		///    current instance as it last appeared on disk.
		/// </value>
		public int Size {
			get {return _sizeOnDisk;}
		}
		
		/// <summary>
		///    Gets and sets the type of value contained in the
		///    current instance.
		/// </summary>
		/// <value>
		///    A <see cref="ItemType" /> value indicating the type of
		///    value contained in the current instance.
		/// </value>
		public ItemType Type {
			get {return _type;}
			set {_type = value;}
		}
		
		/// <summary>
		///    Gets and sets whether or not the current instance is
		///    flagged as read-only on disk.
		/// </summary>
		/// <value>
		///    A <see cref="bool" /> value indicating whether or not the
		///    current instance is flagged as read-only on disk.
		/// </value>
		public bool ReadOnly {
			get {return _readOnly;}
			set {_readOnly = value;}
		}
		
		/// <summary>
		///    Gets whether or not the current instance is empty.
		/// </summary>
		/// <value>
		///    A <see cref="bool" /> value indicating whether or not the
		///    current instance contains no value.
		/// </value>
		public bool IsEmpty {
			get
			{
			    if (_type != ItemType.Binary)
					return _text == null || _text.Length == 0;
			    return _data == null || _data.IsEmpty;
			}
		}
		
		#endregion
		
		
		
		#region Public Methods
		
		/// <summary>
		///    Gets the contents of the current instance as a <see
		///    cref="string" />.
		/// </summary>
		/// <returns>
		///    <para>A <see cref="string" /> object containing the text
		///    stored in the current instance, or <see langword="null"
		///    /> if the item is empty of contains binary data.</para>
		///    <para>If the current instance contains multiple string
		///    values, they will be returned as a comma separated
		///    value.</para>
		/// </returns>
		public override string ToString ()
		{
		    if (_type == ItemType.Binary || _text == null)
				return null;
		    return string.Join (", ", _text);
		}

	    /// <summary>
		///    Gets the contents of the current instance as a <see
		///    cref="string" /> array.
		/// </summary>
		/// <returns>
		///    A <see cref="string[]" /> containing the text stored in
		///    the current instance, or an empty array if the item
		///    contains binary data.
		/// </returns>
		public string [] ToStringArray ()
		{
			if (_type == ItemType.Binary || _text == null)
				return new string [0];
			
			return _text;
		}
		
		/// <summary>
		///    Renders the current instance as an APEv2 item.
		/// </summary>
		/// <returns>
		///    A <see cref="ByteVector" /> object containing the
		///    rendered version of the current instance.
		/// </returns>
		public ByteVector Render ()
		{
			uint flags = (uint) ((ReadOnly) ? 1 : 0) |
				((uint) Type << 1);
			
			if (IsEmpty)
				return new ByteVector ();
			
			ByteVector result = null;
			
			if (_type == ItemType.Binary) {
				if (_text == null && _data != null)
					result = _data;
			}
			
			if (result == null && _text != null) {
				result = new ByteVector ();
				
				for (int i = 0; i < _text.Length; i ++) {
					if (i != 0)
						result.Add (0);
					
					result.Add (ByteVector.FromString (
						_text [i], StringType.UTF8));
				}
			}
			
			// If no data is stored, don't write the item.
			if (result == null || result.Count == 0)
				return new ByteVector ();
			
			ByteVector output = new ByteVector ();
			output.Add (ByteVector.FromUInt ((uint) result.Count,
				false));
			output.Add (ByteVector.FromUInt (flags, false));
			output.Add (ByteVector.FromString (_key, StringType.UTF8));
			output.Add (0);
			output.Add (result);
			
			_sizeOnDisk = output.Count;
			
			return output;
		}
		
		#endregion
		
		#region Protected Methods
		
		/// <summary>
		///    Populates the current instance by reading in a raw APEv2
		///    item.
		/// </summary>
		/// <param name="data">
		///    A <see cref="ByteVector" /> object containing the item to
		///    read.
		/// </param>
		/// <param name="offset">
		///    A <see cref="int" /> value specifying the offset in
		///    <paramref name="data" /> at which the item data begins.
		/// </param>
		/// <exception cref="ArgumentNullException">
		///    <paramref name="data" /> is <see langword="null" />.
		/// </exception>
		/// <exception cref="ArgumentOutOfRangeException">
		///    <paramref name="offset" /> is less than zero.
		/// </exception>
		/// <exception cref="CorruptFileException">
		///    A complete item could not be read.
		/// </exception>
		protected void Parse (ByteVector data, int offset)
		{
			if (data == null)
				throw new ArgumentNullException ("data");
			
			if (offset < 0)
				throw new ArgumentOutOfRangeException ("offset");
			
			
			// 11 bytes is the minimum size for an APE item
			if(data.Count < offset + 11)
				throw new CorruptFileException (
					"Not enough data for APE Item");
			
			uint valueLength = data.Mid (offset, 4).ToUInt (false);
			uint flags = data.Mid (offset + 4, 4).ToUInt (false);
			
			ReadOnly = (flags & 1) == 1;
			Type = (ItemType) ((flags >> 1) & 3);
			
			int pos = data.Find (ByteVector.TextDelimiter (
				StringType.UTF8), offset + 8);
			
			_key = data.ToString (StringType.UTF8,
				offset + 8, pos - offset - 8);
			
			if (valueLength > data.Count - pos - 1)
				throw new CorruptFileException (
					"Invalid data length.");
			
			_sizeOnDisk = pos + 1 + (int) valueLength - offset;
			
			if (Type == ItemType.Binary)
				_data = new ReadOnlyByteVector (
					data.Mid (pos + 1, (int) valueLength));
			else
				_text = data.Mid (pos + 1,
					(int) valueLength).ToStrings (
						StringType.UTF8, 0);
		}
		
#endregion
		
		
		
#region ICloneable
		
		/// <summary>
		///    Creates a deep copy of the current instance.
		/// </summary>
		/// <returns>
		///    A new <see cref="Item"/> object identical to the current
		///    instance.
		/// </returns>
		public Item Clone ()
		{
			return new Item (this);
		}
#endregion
	}
}
