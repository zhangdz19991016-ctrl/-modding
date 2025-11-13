using System;
using System.Diagnostics;
using SodaCraft.StringUtilities;
using UnityEngine;

// Token: 0x02000112 RID: 274
public class NamedFormatTest : MonoBehaviour
{
	// Token: 0x06000965 RID: 2405 RVA: 0x00029DB8 File Offset: 0x00027FB8
	private void Test()
	{
		string message = "";
		Stopwatch stopwatch = Stopwatch.StartNew();
		for (int i = 0; i < this.loopCount; i++)
		{
			message = this.format.Format(this.content);
		}
		stopwatch.Stop();
		UnityEngine.Debug.Log("Time Consumed 1:" + stopwatch.ElapsedMilliseconds.ToString());
		stopwatch = Stopwatch.StartNew();
		for (int j = 0; j < this.loopCount; j++)
		{
			message = string.Format(this.format2, this.content.textA, this.content.textB);
		}
		stopwatch.Stop();
		UnityEngine.Debug.Log("Time Consumed 2:" + stopwatch.ElapsedMilliseconds.ToString());
		UnityEngine.Debug.Log(message);
	}

	// Token: 0x06000966 RID: 2406 RVA: 0x00029E84 File Offset: 0x00028084
	private void Test2()
	{
		Stopwatch stopwatch = Stopwatch.StartNew();
		string message = this.format.Format(new
		{
			this.content.textA,
			this.content.textB
		});
		stopwatch.Stop();
		UnityEngine.Debug.Log("Time Consumed:" + stopwatch.ElapsedMilliseconds.ToString());
		UnityEngine.Debug.Log(message);
	}

	// Token: 0x0400086B RID: 2155
	public string format = "Displaying {textA} {textB}";

	// Token: 0x0400086C RID: 2156
	public string format2 = "Displaying {0} {1}";

	// Token: 0x0400086D RID: 2157
	public NamedFormatTest.Content content;

	// Token: 0x0400086E RID: 2158
	[SerializeField]
	private int loopCount = 100;

	// Token: 0x0200049D RID: 1181
	[Serializable]
	public struct Content
	{
		// Token: 0x04001C19 RID: 7193
		public string textA;

		// Token: 0x04001C1A RID: 7194
		public string textB;
	}
}
