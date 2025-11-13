using System;

// Token: 0x0200006E RID: 110
[Serializable]
public struct ElementFactor
{
	// Token: 0x0600042B RID: 1067 RVA: 0x000127F8 File Offset: 0x000109F8
	public ElementFactor(ElementTypes _type, float _factor)
	{
		this.elementType = _type;
		this.factor = _factor;
	}

	// Token: 0x04000335 RID: 821
	public ElementTypes elementType;

	// Token: 0x04000336 RID: 822
	public float factor;
}
