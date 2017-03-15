using Xamarin.Forms;
using EnumPicker.EnumValue;
using EnumPicker.ViewModels;
using System;

namespace EnumPicker
{
	public partial class EnumPickerPage : ContentPage
	{
		public EnumPickerPage()
		{
			InitializeComponent();
			var vm = new EnumBindableViewModel()
			{
				enumValue = EnumHogeFuga.FugaFuga
			};
			st.BindingContext = vm;
			HogeButton.Clicked += (sender, e) => { onClickHogeButton(); };
		}
		private void onClickHogeButton() 
		{
					
		}
	}
}
