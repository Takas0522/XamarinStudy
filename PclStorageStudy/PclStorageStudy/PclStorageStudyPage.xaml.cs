using Xamarin.Forms;
using System;
using PCLStorage;

namespace PclStorageStudy
{
	public partial class PclStorageStudyPage : ContentPage
	{
		public PclStorageStudyPage()
		{
			InitializeComponent();
			SaveButtonCan.Clicked += OnCanSeeSaveButtonClicked;
			SaveButtonCant.Clicked += OnCantSeeSaveButtonClicked;
		}
		private static readonly string FilePath = "/sdcard/sample";
		private async void OnCanSeeSaveButtonClicked(object sender, EventArgs e) {
			IFolder root = await FileSystem.Current.GetFolderFromPathAsync(FilePath);
			InputFile(root);
		}
		private void OnCantSeeSaveButtonClicked(object sender, EventArgs e)
		{
			IFolder root = FileSystem.Current.LocalStorage;
			InputFile(root);
		}
		private async void  InputFile(IFolder root) { 
			IFile file = await root.CreateFileAsync("sample.txt", CreationCollisionOption.GenerateUniqueName);
			var inputText = SampleText.Text;
			await file.WriteAllTextAsync(inputText);			
		}
	}
}
