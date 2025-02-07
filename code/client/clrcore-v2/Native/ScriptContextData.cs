using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security;

namespace CitizenFX.Core
{
	[StructLayout(LayoutKind.Sequential)]
	[Serializable]
	internal struct fxScriptContext
	{
		public unsafe ulong* functionDataPtr;

		public int numArguments;
		public int numResults;

		[SecuritySafeCritical, MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe void Initialize(ulong* data, int argCount)
		{
			functionDataPtr =  data;
			numArguments = argCount;
			numResults = 0;
		}
	}

	[StructLayout(LayoutKind.Sequential)]
	[Serializable]
	internal unsafe struct RageScriptContext
	{
		public ulong* functionDataPtr;
		public int numArguments;
		public int pad;

		public ulong* retDataPtr;

		public int numResults;
		public int pad2;
		public fixed long copyToVectors[4];
		public fixed float vectorData[4 * 4];

		public fixed byte padding[96];

		[SecuritySafeCritical, MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe void Initialize(ulong* data, int argCount)
		{
			functionDataPtr = retDataPtr = data;
			numArguments = argCount;
			numResults = 0;
		}

		[SecuritySafeCritical, MethodImpl(MethodImplOptions.AggressiveInlining)]
		internal unsafe static void CopyVectorData(RageScriptContext* ctx)
		{
			Vector3** dst = (Vector3**)ctx->copyToVectors;
			Vector4* src = (Vector4*)ctx->vectorData;
			Vector4* end = src + ctx->numResults;

			for (; src < end; src++)
				**dst++ = (Vector3)(*src);
		}
	}
}
