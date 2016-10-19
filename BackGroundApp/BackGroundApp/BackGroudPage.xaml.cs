using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace BackGroundApp
{
	public partial class BackGroudPage : ContentPage
	{
		public BackGroudPage()
		{
			InitializeComponent();
		}
		public DateTime SleepDate
		{
			set
			{
				this.SleepDateLabel.Text = value.ToString("t");
			}
		}

		public string FirstName
		{
			get
			{
				return this.FirstNameEntry.Text;
			}
			set
			{
				this.FirstNameEntry.Text = value;
			}
		}
	}
}
