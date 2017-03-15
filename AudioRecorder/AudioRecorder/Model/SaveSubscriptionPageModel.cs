using System;
namespace AudioRecorder.Model
{
	public class SaveSubscriptionPageModel:BindableBase
	{
		private string _subscriptionKey;
		public string SubscriptionKey
		{ 
			get 
			{
				return _subscriptionKey;
			}
			set
			{
				_subscriptionKey = value;
			}
		}
	}
}
