using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RedatamLib
{
	public class NullCursorReader : ICursorReader
	{
		public void Close()
		{
		}

		public bool IsLastPos()
		{
			return false;
		}

		public void Open()
		{
		}

		public double ReadDouble()
		{
			throw new NotImplementedException();
		}

		public uint ReadInt16()
		{
			throw new NotImplementedException();
		}

		public uint ReadInt32()
		{
			return 0;
		}

		public uint ReadLastInt32()
		{
			return 0;
		}

		public ulong ReadNumber()
		{
			return 0;
		}

		public string ReadString()
		{
			return "0";
		}


		public long Length
		{
			get
			{
				return 0;
			}
		}
	}
}
