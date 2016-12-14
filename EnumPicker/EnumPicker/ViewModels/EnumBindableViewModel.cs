using System;
using System.ComponentModel;
using EnumPicker.EnumValue;
namespace EnumPicker.ViewModels
{
	public class EnumBindableViewModel: BindableBase
	{
		private EnumHogeFuga _enumValue;
		public EnumHogeFuga enumValue 
		{ 
			get { return _enumValue; }
			set { SetProperty(ref _enumValue, value); }
		}
	}
}
