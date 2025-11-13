using System;

// Token: 0x020001C8 RID: 456
public class ScrollWheelOptionsProvider : OptionsProviderBase
{
	// Token: 0x1700027E RID: 638
	// (get) Token: 0x06000DA6 RID: 3494 RVA: 0x00038AFE File Offset: 0x00036CFE
	public override string Key
	{
		get
		{
			return "Input_ScrollWheelBehaviour";
		}
	}

	// Token: 0x06000DA7 RID: 3495 RVA: 0x00038B05 File Offset: 0x00036D05
	public override string GetCurrentOption()
	{
		return ScrollWheelBehaviour.GetDisplayName(ScrollWheelBehaviour.CurrentBehaviour);
	}

	// Token: 0x06000DA8 RID: 3496 RVA: 0x00038B14 File Offset: 0x00036D14
	public override string[] GetOptions()
	{
		ScrollWheelBehaviour.Behaviour[] array = (ScrollWheelBehaviour.Behaviour[])Enum.GetValues(typeof(ScrollWheelBehaviour.Behaviour));
		string[] array2 = new string[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array2[i] = ScrollWheelBehaviour.GetDisplayName(array[i]);
		}
		return array2;
	}

	// Token: 0x06000DA9 RID: 3497 RVA: 0x00038B59 File Offset: 0x00036D59
	public override void Set(int index)
	{
		ScrollWheelBehaviour.CurrentBehaviour = ((ScrollWheelBehaviour.Behaviour[])Enum.GetValues(typeof(ScrollWheelBehaviour.Behaviour)))[index];
	}
}
