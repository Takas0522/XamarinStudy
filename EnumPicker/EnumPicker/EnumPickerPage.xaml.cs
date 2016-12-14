using Xamarin.Forms;
using EnumPicker.EnumValue;
using System;

namespace EnumPicker
{
	public partial class EnumPickerPage : ContentPage
	{
		public EnumPickerPage()
		{
			InitializeComponent();
			HogeButton.Clicked += (sender, e) => { onClickHogeButton(); };
		}
		private void onClickHogeButton() 
		{
				
		}
	}
}
