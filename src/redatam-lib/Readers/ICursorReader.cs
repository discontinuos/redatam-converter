using System;
namespace RedatamLib
{
	public interface ICursorReader
	{
		void Close();
		bool IsLastPos();
		void Open();
		double ReadDouble();
		uint ReadInt16();
		uint ReadInt32();
		uint ReadLastInt32();
		ulong ReadNumber();
		string ReadString();

		long Length { get; }
	}
}
