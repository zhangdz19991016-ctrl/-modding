using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace Duckov.MiniMaps
{
	// Token: 0x02000279 RID: 633
	public static class PointsOfInterests
	{
		// Token: 0x1700039E RID: 926
		// (get) Token: 0x0600140C RID: 5132 RVA: 0x0004AD81 File Offset: 0x00048F81
		public static ReadOnlyCollection<MonoBehaviour> Points
		{
			get
			{
				if (PointsOfInterests.points_ReadOnly == null)
				{
					PointsOfInterests.points_ReadOnly = new ReadOnlyCollection<MonoBehaviour>(PointsOfInterests.points);
				}
				return PointsOfInterests.points_ReadOnly;
			}
		}

		// Token: 0x14000085 RID: 133
		// (add) Token: 0x0600140D RID: 5133 RVA: 0x0004ADA0 File Offset: 0x00048FA0
		// (remove) Token: 0x0600140E RID: 5134 RVA: 0x0004ADD4 File Offset: 0x00048FD4
		public static event Action<MonoBehaviour> OnPointRegistered;

		// Token: 0x14000086 RID: 134
		// (add) Token: 0x0600140F RID: 5135 RVA: 0x0004AE08 File Offset: 0x00049008
		// (remove) Token: 0x06001410 RID: 5136 RVA: 0x0004AE3C File Offset: 0x0004903C
		public static event Action<MonoBehaviour> OnPointUnregistered;

		// Token: 0x06001411 RID: 5137 RVA: 0x0004AE6F File Offset: 0x0004906F
		public static void Register(MonoBehaviour point)
		{
			PointsOfInterests.points.Add(point);
			Action<MonoBehaviour> onPointRegistered = PointsOfInterests.OnPointRegistered;
			if (onPointRegistered != null)
			{
				onPointRegistered(point);
			}
			PointsOfInterests.CleanUp();
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x0004AE92 File Offset: 0x00049092
		public static void Unregister(MonoBehaviour point)
		{
			if (PointsOfInterests.points.Remove(point))
			{
				Action<MonoBehaviour> onPointUnregistered = PointsOfInterests.OnPointUnregistered;
				if (onPointUnregistered != null)
				{
					onPointUnregistered(point);
				}
			}
			PointsOfInterests.CleanUp();
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x0004AEB7 File Offset: 0x000490B7
		private static void CleanUp()
		{
			PointsOfInterests.points.RemoveAll((MonoBehaviour e) => e == null);
		}

		// Token: 0x04000EC9 RID: 3785
		private static List<MonoBehaviour> points = new List<MonoBehaviour>();

		// Token: 0x04000ECA RID: 3786
		private static ReadOnlyCollection<MonoBehaviour> points_ReadOnly;
	}
}
