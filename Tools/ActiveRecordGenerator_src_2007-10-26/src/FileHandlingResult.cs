using System;
using System.Collections.Generic;
using System.Text;

namespace ActiveRecordGenerator
{
	[Flags]
	public enum FileHandlingResult
	{
		None = 0x00,
		OverWrite = 0x01,
		Rename = 0x02,
		Skip = 0x04,
		Cancel = 0x08,
		All = 0x10
	}
}
