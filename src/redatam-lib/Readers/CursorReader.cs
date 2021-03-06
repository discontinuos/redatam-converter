﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace RedatamLib
{
	public class CursorReader : ICursorReader
	{
		public int BlockSize;
		public string Filename;
		public bool IsString;
		public bool IsBinaryDataSet;

		private FileStream stream;
		private long fileSize;
		uint trailingBits = 0;
		int trailingBitsCount = 0;
		int bytesPos = 0;
		
		public CursorReader(string file, int size):
					this(file, false, false, size)
		{
		}
		public CursorReader(string file, bool isString, bool isBin, int size)
		{
			IsString = isString;
			Filename = file;
			IsBinaryDataSet = isBin;
			BlockSize = size;
		}
		public void Open()
		{
			fileSize = new FileInfo(Filename).Length;
			stream = File.OpenRead(Filename);
			bytesPos = 0;

		}
		public string ReadString()
		{
			byte[] bytes = new byte[BlockSize];
			stream.Read(bytes, 0, BlockSize);
			return System.Text.Encoding.Default.GetString(bytes);
		}
		public double ReadDouble()
		{
			byte[] bytes = new byte[8];
			for(int i = 0; i < 8; i++)
				bytes[i] = (byte) ReadByte();
			double ret = BitConverter.ToDouble(bytes, 0);
			return ret;
		}
		public ulong ReadNumber()
		{
			// lee de a bloques de bits
			int bitsRead = 0;
			ulong data = 0;
			// lee el siguiente byte
			data = feedBits(data, ref bitsRead);
			while (bitsRead < BlockSize)
			{
				if (IsBinaryDataSet)
					trailingBits = this.Read4Bytes();
				else
					trailingBits = this.ReadInt32();
				bytesPos += 4;
				trailingBitsCount = 32;
				data = feedBits(data, ref bitsRead);
			}
			return data;
		}

		private uint Read4Bytes()
		{
			return Read2Bytes() * 0x10000 + Read2Bytes();
		}
		private uint Read2Bytes()
		{
			return ReadByte() * 0x100 + ReadByte();
		}

		private ulong feedBits(ulong data, ref int bitsRead)
		{
			while (bitsRead < BlockSize && trailingBitsCount > 0)
			{
				// hace lugar
				data <<= 1;
				// agregar el bit
				uint bit = (trailingBits >> 31) & 1;
				data += bit;
				// actualiza contadores
				trailingBits <<= 1;
				trailingBitsCount--;
				bitsRead++;
			}
			return data;
		}
		public void Close()
		{
			stream.Close();
			stream.Dispose();
		}

		public bool IsLastPos()
		{
			return bytesPos >= fileSize;
		}

		public uint ReadInt32()
		{
			return ReadInt16() + ReadInt16() * 0x10000;
		}

		public uint ReadInt16()
		{
			return ReadByte() + ReadByte() * 0x100;
		}

		private uint ReadByte()
		{
			int ret = stream.ReadByte();
			if (ret == -1)
			{
				if (stream.Position == stream.Length)
				{
					return 0;
				}
				else
					throw new Exception("No se pudo leer el archivo de origen.");
			}
			return (uint)ret;
		}


		public long Length
		{
			get
			{
				return stream.Length;
			}
	 }

		public uint ReadInt32At(long pos)
		{
			long keepPos = stream.Position;
			stream.Position = pos * 4;
			uint ret = this.ReadInt32();
			stream.Position = keepPos;
			return ret;
		}
		public uint ReadLastInt32()
		{
			long keepPos = stream.Position;
			stream.Position = stream.Length - 4;
			uint ret = this.ReadInt32();
			stream.Position = keepPos;
			return ret;
		}
	}
}
