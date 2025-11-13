using System;
using System.IO;
using Saves;

namespace Duckov
{
	// Token: 0x02000241 RID: 577
	public class CheatMode
	{
		// Token: 0x1700031C RID: 796
		// (get) Token: 0x060011FF RID: 4607 RVA: 0x0004593C File Offset: 0x00043B3C
		// (set) Token: 0x06001200 RID: 4608 RVA: 0x00045943 File Offset: 0x00043B43
		public static bool Active
		{
			get
			{
				return CheatMode._acitive;
			}
			private set
			{
				CheatMode._acitive = value;
				Action<bool> onCheatModeStatusChanged = CheatMode.OnCheatModeStatusChanged;
				if (onCheatModeStatusChanged == null)
				{
					return;
				}
				onCheatModeStatusChanged(value);
			}
		}

		// Token: 0x1400007B RID: 123
		// (add) Token: 0x06001201 RID: 4609 RVA: 0x0004595C File Offset: 0x00043B5C
		// (remove) Token: 0x06001202 RID: 4610 RVA: 0x00045990 File Offset: 0x00043B90
		public static event Action<bool> OnCheatModeStatusChanged;

		// Token: 0x06001203 RID: 4611 RVA: 0x000459C3 File Offset: 0x00043BC3
		public static void Activate()
		{
			if (!CheatMode.CheatFileExists())
			{
				return;
			}
			CheatMode.Active = true;
			SavesSystem.Save<bool>("Cheated", true);
		}

		// Token: 0x06001204 RID: 4612 RVA: 0x000459DE File Offset: 0x00043BDE
		public static void Deactivate()
		{
			CheatMode.Active = false;
		}

		// Token: 0x1700031D RID: 797
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x000459E6 File Offset: 0x00043BE6
		private bool Cheated
		{
			get
			{
				return SavesSystem.Load<bool>("Cheated");
			}
		}

		// Token: 0x06001206 RID: 4614 RVA: 0x000459F2 File Offset: 0x00043BF2
		private static bool CheatFileExists()
		{
			return File.Exists(Path.Join(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "WWSSADADBA"));
		}

		// Token: 0x04000DDE RID: 3550
		private static bool _acitive;
	}
}
