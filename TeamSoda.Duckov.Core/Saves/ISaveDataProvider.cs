using System;

namespace Saves
{
	// Token: 0x02000226 RID: 550
	public interface ISaveDataProvider
	{
		// Token: 0x0600109D RID: 4253
		object GenerateSaveData();

		// Token: 0x0600109E RID: 4254
		void SetupSaveData(object data);
	}
}
