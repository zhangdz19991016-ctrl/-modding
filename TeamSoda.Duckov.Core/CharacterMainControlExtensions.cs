using System;

// Token: 0x02000098 RID: 152
public static class CharacterMainControlExtensions
{
	// Token: 0x06000533 RID: 1331 RVA: 0x000178E8 File Offset: 0x00015AE8
	public static bool IsMainCharacter(this CharacterMainControl character)
	{
		if (character == null)
		{
			return false;
		}
		LevelManager instance = LevelManager.Instance;
		return ((instance != null) ? instance.MainCharacter : null) == character;
	}
}
