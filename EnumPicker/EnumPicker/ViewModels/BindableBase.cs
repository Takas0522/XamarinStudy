﻿using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
public class BindableBase : INotifyPropertyChanged
{
	public event PropertyChangedEventHandler PropertyChanged;

	protected virtual void OnPropertyChanged([CallerMemberName]string propertyName = null) =>
		this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

	protected virtual bool SetProperty<T>(ref T field, T value, [CallerMemberName]string propertyName = null)
	{
		if (EqualityComparer<T>.Default.Equals(field, value)) { return false; }
		field = value;
		this.OnPropertyChanged(propertyName);
		return true;
	}
}