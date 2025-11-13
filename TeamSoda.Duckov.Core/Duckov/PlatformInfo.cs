using System;
using UnityEngine;

namespace Duckov
{
	// Token: 0x0200023D RID: 573
	public static class PlatformInfo
	{
		// Token: 0x17000319 RID: 793
		// (get) Token: 0x060011EE RID: 4590 RVA: 0x000457F8 File Offset: 0x000439F8
		// (set) Token: 0x060011EF RID: 4591 RVA: 0x0004580D File Offset: 0x00043A0D
		public static Platform Platform
		{
			get
			{
				if (Application.isEditor)
				{
					return Platform.UnityEditor;
				}
				return GameMetaData.Instance.Platform;
			}
			set
			{
				GameMetaData.Instance.Platform = value;
			}
		}

		// Token: 0x1700031A RID: 794
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x0004581A File Offset: 0x00043A1A
		// (set) Token: 0x060011F1 RID: 4593 RVA: 0x00045821 File Offset: 0x00043A21
		public static Func<string> GetIDFunc
		{
			get
			{
				return PlatformInfo._getIDFunc;
			}
			set
			{
				PlatformInfo._getIDFunc = value;
			}
		}

		// Token: 0x1700031B RID: 795
		// (get) Token: 0x060011F2 RID: 4594 RVA: 0x00045829 File Offset: 0x00043A29
		// (set) Token: 0x060011F3 RID: 4595 RVA: 0x00045830 File Offset: 0x00043A30
		public static Func<string> GetDisplayNameFunc
		{
			get
			{
				return PlatformInfo._getDisplayNameFunc;
			}
			set
			{
				PlatformInfo._getDisplayNameFunc = value;
			}
		}

		// Token: 0x060011F4 RID: 4596 RVA: 0x00045838 File Offset: 0x00043A38
		public static string GetID()
		{
			string text = null;
			if (PlatformInfo.GetIDFunc != null)
			{
				text = PlatformInfo.GetIDFunc();
			}
			if (text == null)
			{
				text = Environment.MachineName;
			}
			return text;
		}

		// Token: 0x060011F5 RID: 4597 RVA: 0x00045863 File Offset: 0x00043A63
		public static string GetDisplayName()
		{
			if (PlatformInfo.GetDisplayNameFunc != null)
			{
				return PlatformInfo.GetDisplayNameFunc();
			}
			return "UNKOWN";
		}

		// Token: 0x04000DD1 RID: 3537
		private static Func<string> _getIDFunc;

		// Token: 0x04000DD2 RID: 3538
		private static Func<string> _getDisplayNameFunc;
	}
}
