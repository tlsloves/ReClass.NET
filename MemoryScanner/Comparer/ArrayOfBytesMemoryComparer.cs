﻿using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;

namespace ReClassNET.MemoryScanner.Comparer
{
	public class ArrayOfBytesMemoryComparer : IScanComparer
	{
		public ScanCompareType CompareType => ScanCompareType.Equal;
		public BytePattern Value { get; }
		public int ValueSize => Value.Length;

		private readonly byte[] pattern;

		public ArrayOfBytesMemoryComparer(BytePattern value)
		{
			Contract.Requires(value != null);

			Value = value;

			if (!value.HasWildcards)
			{
				pattern = value.ToByteArray();
			}
		}

		public bool Compare(byte[] data, int index, out ScanResult result)
		{
			result = null;

			if (pattern != null)
			{
				for (var i = 0; i < pattern.Length; ++i)
				{
					if (data[index + i] != pattern[i])
					{
						return false;
					}
				}
			}
			else if (!Value.Equals(data, index))
			{
				return false;
			}

			var temp = new byte[ValueSize];
			Array.Copy(data, index, temp, 0, temp.Length);
			result = new ArrayOfBytesScanResult(temp);

			return true;
		}

		public bool Compare(byte[] data, int index, ScanResult previous, out ScanResult result)
		{
#if DEBUG
			Debug.Assert(previous is ArrayOfBytesScanResult);
#endif

			return Compare(data, index, out result);
		}
	}
}
