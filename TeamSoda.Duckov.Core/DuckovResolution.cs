using System;
using UnityEngine;

// Token: 0x020001CF RID: 463
[Serializable]
public struct DuckovResolution
{
	// Token: 0x06000DD1 RID: 3537 RVA: 0x0003923C File Offset: 0x0003743C
	public override bool Equals(object obj)
	{
		if (obj is DuckovResolution)
		{
			DuckovResolution duckovResolution = (DuckovResolution)obj;
			if (duckovResolution.height == this.height && duckovResolution.width == this.width)
			{
				return true;
			}
		}
		return false;
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x00039277 File Offset: 0x00037477
	public override string ToString()
	{
		return string.Format("{0} x {1}", this.width, this.height);
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x00039299 File Offset: 0x00037499
	public DuckovResolution(Resolution res)
	{
		this.height = res.height;
		this.width = res.width;
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x000392B5 File Offset: 0x000374B5
	public DuckovResolution(int _width, int _height)
	{
		this.height = _height;
		this.width = _width;
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x000392C8 File Offset: 0x000374C8
	public bool CheckRotioFit(DuckovResolution newRes, DuckovResolution defaultRes)
	{
		float num = (float)newRes.height / (float)newRes.width;
		return Mathf.Abs((float)defaultRes.height - num * (float)defaultRes.width) <= 2f;
	}

	// Token: 0x04000BBD RID: 3005
	public int width;

	// Token: 0x04000BBE RID: 3006
	public int height;
}
