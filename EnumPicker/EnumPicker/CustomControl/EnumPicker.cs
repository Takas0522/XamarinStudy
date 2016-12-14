using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
namespace EnumPicker.CustomControl
{
	public class EnumPicker<T> : Picker where T : struct
	{
		public EnumPicker()
		{
			//ベースのイベントに加えて独自イベントも行う
			SelectedIndexChanged += OnSelectedIndexChanged;
			foreach (var value in Enum.GetValues(typeof(T)))
			{
				//Generic型＜T＞のEnum型からValueを作成する。
				Items.Add(GetEnumDescription(value));
			}
		}

		/// <summary>
		/// バインドプロパティの作成
		/// </summary>
		public static BindableProperty SelectedItemProperty = 
			BindableProperty.Create(
				nameof(SelectedItem), //PropertyName
				typeof(T), //PropertyType
				typeof(EnumPicker<T>), //ViewPropertyType
				default(T), //defaultValue
				propertyChanged: OnSelectedItemChanged, //ItemChangedEvent
				defaultBindingMode: BindingMode.TwoWay
		);

		/// <summary>
		/// Generic<T>の型を登録する
		/// </summary>
		/// <value>The selected item.</value>
		public T SelectedItem { 
			get { return (T)GetValue(SelectedItemProperty); }
			set {
				SetValue(SelectedItemProperty, value);
			}
		}

		/// <summary>
		/// 値変更時イベント
		/// </summary>
		/// <param name="sender">Sender.</param>
		/// <param name="e">E.</param>
		private void OnSelectedIndexChanged(object sender, EventArgs e) {
			//Pickerで登録された値を元に、Enumの値を取得。
			if (SelectedIndex < 0 || SelectedIndex > Items.Count - 1)
			{
				SelectedItem = default(T);
			}
			else {
				T match;
				if (!Enum.TryParse<T>(Items[SelectedIndex], out match)) {
					match = GetEnumByDescription(Items[SelectedIndex]);
				}
				SelectedItem = (T)Enum.Parse(typeof(T), match.ToString());
			}
		}

		/// <summary>
		/// BindObjectが変更された際に発生？
		/// </summary>
		private static void OnSelectedItemChanged(BindableObject bindable, object oldValue, object newValue) {
			var picker = bindable as EnumPicker<T>;
			//PickeのSelectedIndexを変更されたEnumの値を元に変更
			if (newValue != null) {
				picker.SelectedIndex = picker.Items.IndexOf(GetEnumDescription(newValue));
			}
		}

		/// <summary>
		/// enumに設定されている日本語文字列の取得
		/// </summary>
		private static string GetEnumDescription(object value) {
			string result = value.ToString();
			DisplayAttribute attribute = typeof(T).GetRuntimeField(value.ToString()).GetCustomAttributes<DisplayAttribute>(false).SingleOrDefault();
			if (attribute != null) {
				result = attribute.Description;
			}
			return result;
		}

		/// <summary>
		/// 入力文字列からEnum値を特定
		/// </summary>>
		private T GetEnumByDescription(string description) {
			return Enum.GetValues(typeof(T)).Cast<T>().FirstOrDefault(x => string.Equals(GetEnumDescription(x), description));
		}
	}
}
