using System;
using UnityEngine;

namespace Duckov.MiniGames.BubblePoppers
{
	// Token: 0x020002E1 RID: 737
	public class BubblePopperLevelDataProvider : MonoBehaviour
	{
		// Token: 0x17000435 RID: 1077
		// (get) Token: 0x060017B2 RID: 6066 RVA: 0x000577DC File Offset: 0x000559DC
		public int TotalLevels
		{
			get
			{
				return this.totalLevels;
			}
		}

		// Token: 0x060017B3 RID: 6067 RVA: 0x000577E4 File Offset: 0x000559E4
		internal int[] GetData(int levelIndex)
		{
			int num = this.seed + levelIndex;
			int[] array = new int[60 + 10 * (levelIndex / 2)];
			System.Random random = new System.Random(num);
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = random.Next(0, this.master.AvaliableColorCount);
			}
			return array;
		}

		// Token: 0x0400114E RID: 4430
		[SerializeField]
		private BubblePopper master;

		// Token: 0x0400114F RID: 4431
		[SerializeField]
		private int totalLevels = 10;

		// Token: 0x04001150 RID: 4432
		[SerializeField]
		public int seed;
	}
}
