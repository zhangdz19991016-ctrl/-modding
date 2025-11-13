using System;
using System.Collections.Generic;
using UnityEngine;

namespace Duckov
{
	// Token: 0x02000233 RID: 563
	public class CursorManager : MonoBehaviour
	{
		// Token: 0x17000308 RID: 776
		// (get) Token: 0x0600119E RID: 4510 RVA: 0x00044CB8 File Offset: 0x00042EB8
		// (set) Token: 0x0600119F RID: 4511 RVA: 0x00044CBF File Offset: 0x00042EBF
		public static CursorManager Instance { get; private set; }

		// Token: 0x060011A0 RID: 4512 RVA: 0x00044CC7 File Offset: 0x00042EC7
		public static void Register(ICursorDataProvider dataProvider)
		{
			CursorManager.cursorDataStack.Add(dataProvider);
			CursorManager.ApplyStackData();
		}

		// Token: 0x060011A1 RID: 4513 RVA: 0x00044CD9 File Offset: 0x00042ED9
		public static bool Unregister(ICursorDataProvider dataProvider)
		{
			if (CursorManager.cursorDataStack.Count < 1)
			{
				return false;
			}
			if (!CursorManager.cursorDataStack.Contains(dataProvider))
			{
				return false;
			}
			bool result = CursorManager.cursorDataStack.Remove(dataProvider);
			CursorManager.ApplyStackData();
			return result;
		}

		// Token: 0x060011A2 RID: 4514 RVA: 0x00044D0C File Offset: 0x00042F0C
		private static void ApplyStackData()
		{
			if (CursorManager.Instance == null)
			{
				return;
			}
			if (CursorManager.cursorDataStack.Count <= 0)
			{
				CursorManager.Instance.MSetDefaultCursor();
				return;
			}
			ICursorDataProvider cursorDataProvider = CursorManager.cursorDataStack[CursorManager.cursorDataStack.Count - 1];
			if (cursorDataProvider == null)
			{
				CursorManager.Instance.MSetDefaultCursor();
			}
			CursorManager.Instance.MSetCursor(cursorDataProvider.GetCursorData());
		}

		// Token: 0x060011A3 RID: 4515 RVA: 0x00044D73 File Offset: 0x00042F73
		private void Awake()
		{
			CursorManager.Instance = this;
			this.MSetCursor(this.defaultCursor);
		}

		// Token: 0x060011A4 RID: 4516 RVA: 0x00044D88 File Offset: 0x00042F88
		private void Update()
		{
			if (this.currentCursor == null)
			{
				return;
			}
			if (this.currentCursor.textures.Length < 2)
			{
				return;
			}
			this.fpsBuffer += Time.unscaledDeltaTime * this.currentCursor.fps;
			if (this.fpsBuffer > 1f)
			{
				this.fpsBuffer = 0f;
				this.frame++;
				this.RefreshCursor();
			}
		}

		// Token: 0x060011A5 RID: 4517 RVA: 0x00044DF9 File Offset: 0x00042FF9
		private void RefreshCursor()
		{
			if (this.currentCursor == null)
			{
				return;
			}
			this.currentCursor.Apply(this.frame);
		}

		// Token: 0x060011A6 RID: 4518 RVA: 0x00044E15 File Offset: 0x00043015
		public void MSetDefaultCursor()
		{
			this.MSetCursor(this.defaultCursor);
		}

		// Token: 0x060011A7 RID: 4519 RVA: 0x00044E23 File Offset: 0x00043023
		public void MSetCursor(CursorData data)
		{
			this.currentCursor = data;
			this.frame = 12;
			this.RefreshCursor();
		}

		// Token: 0x060011A8 RID: 4520 RVA: 0x00044E3C File Offset: 0x0004303C
		private void OnDestroy()
		{
			Cursor.SetCursor(null, default(Vector2), CursorMode.Auto);
		}

		// Token: 0x060011A9 RID: 4521 RVA: 0x00044E59 File Offset: 0x00043059
		internal static void NotifyRefresh()
		{
			CursorManager.ApplyStackData();
		}

		// Token: 0x04000DAD RID: 3501
		[SerializeField]
		private CursorData defaultCursor;

		// Token: 0x04000DAE RID: 3502
		public CursorData currentCursor;

		// Token: 0x04000DAF RID: 3503
		private static List<ICursorDataProvider> cursorDataStack = new List<ICursorDataProvider>();

		// Token: 0x04000DB0 RID: 3504
		private int frame;

		// Token: 0x04000DB1 RID: 3505
		private float fpsBuffer;
	}
}
