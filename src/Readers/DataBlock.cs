using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Spss;

namespace RedatamConverter
{
	class DataBlock
	{
		public byte[] data;
		public int n =0;
		private string path;

		public DataBlock(string path)
		{
			data = File.ReadAllBytes(path);
		}
		public DataBlock(byte[] bytes)
		{
			data = bytes;
		}

		public DataBlock getPart(int prevStart, int iStart)
		{
			byte[] part = new byte[iStart - prevStart];
			Array.Copy(data, prevStart, part, 0, iStart - prevStart);
			return new DataBlock(part);
		}
		public bool eatPlausibleString(out string cad, bool filterByContent = true)
		{
			if (PlausibleString(out cad, filterByContent) == false)
				return false;

			cad = eatShortString();
			return true;
		}
		public bool PlausibleString(out string cad, bool filterByContent = true)
		{
			int keepN = n;
			cad = "";
			if (n + 2 >= data.Length)
				return false;
			int length = eat16int();
			if (length < 0 || length > 128 || n + length > data.Length)
			{
				n = keepN;
				return false;
			}	// lo lee...
			move(-2);
			cad = eatShortString();
			n = keepN;

			if (filterByContent == true && IsText(cad) == false) return false;
			return true;
		}
		public List<int> GetAllMatches(byte[] block)
		{
			List<int> ret = new List<int>();
			int keepN = n;
			while (moveTo(block))
			{
				ret.Add(n);
				n++;
			}
			n = keepN;
			return ret;
		}

		public bool IsText(string cad)
		{
			foreach (char c in cad.ToLower())
			{
				if ((c < 'a' || c > 'z') && c != ' ' && c != '-' && c != '_' &&
				 (c < '0' || c > '9'))
					return false;
			}
			return true;
		}
		public bool moveBackString()
		{
			move(-2);
			int offset = 0;
			while(offset < n)
			{
				var bytes = calcSize16(offset);
				if (matches(data, bytes, n - offset))
				{
					n -= offset;
					return true;
				}
				offset++;
			}
			return false;
		}

		public byte[] makeStringBlock(string entity)
		{
			byte[] intSize = calcSize16(entity);
			var text = makeString(entity);
			var block = addArrays(intSize, text);
			return block;
		}

		private static byte[] makeString(string entity)
		{
			var text = System.Text.Encoding.Default.GetBytes(entity);

			return text;
		}

		byte[] calcSize(string cad)
		{
			int i = cad.Length;
			return addArrays(calcSize16(i / 65536), calcSize16(i % 65536));
		}
		byte[] calcSize16(string cad)
		{
			int i = cad.Length;
			return  calcSize16(i);
		}
		string eatString()
		{
			int length = eat32intInv();
			return eatChars(length);
		}
		public string eatShortString()
		{
			int length = eat16int();
			if (length == 0xFFFF)
			{
				return eatString();
			}
			else
				return eatChars(length);
		}
		int eat32intInv()
		{
			return eat16int() + 0x10000 * eat16int();
		}

		public int eat32int()
		{
			return  0x10000 * eat16int() + eat16int() ;
		}
		public int eat16int()
		{
			return 0x1 * eatByte() +
				0x100 * eatByte();
		}
		public byte eatByte()
		{
			return data[n++];
		}
		string eatChars(int length)
		{
			string cad = Encoding.Default.GetString(data, n, length);
			n += length;
			return cad;
		}
		public byte[] addArrays(byte[] a, byte[] b, byte[] c)
		{
			return addArrays(addArrays(a, b), c);
		}
		public byte[] addArrays(byte[] a, byte[] b)
		{
			byte[] ret = new byte[a.Length + b.Length];
			a.CopyTo(ret, 0);
			b.CopyTo(ret, a.Length);
			return ret;
		}
		byte[] calcSize16(int n)
		{
			byte n1 = (byte) (n % 256);
			byte n2 = (byte ) (n / 256);
			return new byte[] { n1, n2 };
		}
		void skipLine()
		{
			const string EOL = "\r\n";
			//moveTo(EOL);
		}
		public bool moveTo(string item)
		{
			return moveTo(makeString(item));
		}
		public bool moveTo(byte[] item)
		{
			int ret = SearchBytes(data, item, n);
			if (ret == -1)
				return false;
			n=ret;
			return true;
		}
		public void move(int i)
		{
			n += i;
		}
		int SearchBytes(byte[] haystack, byte[] needle, int start = 0)
		{
			var len = needle.Length;
			var limit = haystack.Length - len;
			for (var i = start; i <= limit; i++)
			{
				if (matches(haystack, needle, i))
					return i;
			}
			return -1;
		}

		private bool matches(byte[] haystack, byte[] needle, int offset)
		{
			var len = needle.Length;
			for (var k = 0; k < len; k++)
			{
				if (needle[k] != haystack[offset + k]) return false;
			}
			return true;
		}
	}
}
