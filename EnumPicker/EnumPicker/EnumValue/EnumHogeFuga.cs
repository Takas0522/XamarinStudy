using System;
using System.ComponentModel.DataAnnotations;
namespace EnumPicker.EnumValue
{
public enum EnumHogeFuga
	{
		[Display(Description = "ホゲホゲ")]
		HogeHoge,
		[Display(Description = "フガフガ")]
		FugaFuga,
		[Display(Description = "ヘゴヘゴ")]
		HegoHego,
		[Display(Description = "クラクラ")]
		KuraKura
	}
}
