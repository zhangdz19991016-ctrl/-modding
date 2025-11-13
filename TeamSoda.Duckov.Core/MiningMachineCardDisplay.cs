using System;
using UnityEngine;

// Token: 0x02000182 RID: 386
public class MiningMachineCardDisplay : MonoBehaviour
{
	// Token: 0x06000BAF RID: 2991 RVA: 0x00031CA8 File Offset: 0x0002FEA8
	public void SetVisualActive(bool active, MiningMachineCardDisplay.CardTypes cardType)
	{
		this.activeVisual.SetActive(active);
		this.deactiveVisual.SetActive(!active);
		if (cardType == MiningMachineCardDisplay.CardTypes.normal)
		{
			this.normalGPU.SetActive(true);
			this.potatoGPU.SetActive(false);
			return;
		}
		if (cardType != MiningMachineCardDisplay.CardTypes.potato)
		{
			throw new ArgumentOutOfRangeException("cardType", cardType, null);
		}
		this.normalGPU.SetActive(false);
		this.potatoGPU.SetActive(true);
	}

	// Token: 0x04000A00 RID: 2560
	public GameObject activeVisual;

	// Token: 0x04000A01 RID: 2561
	public GameObject deactiveVisual;

	// Token: 0x04000A02 RID: 2562
	public GameObject normalGPU;

	// Token: 0x04000A03 RID: 2563
	public GameObject potatoGPU;

	// Token: 0x020004C1 RID: 1217
	public enum CardTypes
	{
		// Token: 0x04001CC8 RID: 7368
		normal,
		// Token: 0x04001CC9 RID: 7369
		potato
	}
}
