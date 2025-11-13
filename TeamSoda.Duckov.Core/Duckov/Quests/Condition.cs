using System;
using UnityEngine;

namespace Duckov.Quests
{
	// Token: 0x02000336 RID: 822
	public class Condition : MonoBehaviour
	{
		// Token: 0x06001C07 RID: 7175 RVA: 0x00066108 File Offset: 0x00064308
		public virtual bool Evaluate()
		{
			return false;
		}

		// Token: 0x1700051E RID: 1310
		// (get) Token: 0x06001C08 RID: 7176 RVA: 0x0006610B File Offset: 0x0006430B
		public virtual string DisplayText
		{
			get
			{
				return "";
			}
		}
	}
}
