# Xamarin.Formsでバックグラウンドアプリを作る

## 参考

http://arteksoftware.com/backgrounding-with-xamarin-forms/

基本的には上のサイトの内容をなぞっているだけなので

英語余裕だぜって方は上をどうぞ。

## 実現方法とは

> * Persisting some state quickly when the user sends our app to the background

> * Long running, or finite length, tasks that are a normal part of our app’s logic. These are important tasks that we don’t want to be interrupted when the app is sent to the background, but we want to continue to run our code without terminating our app.

> * Downloading a file, which is handled as a Background Necessary Application in iOS, and a Service in Android

* ユーザーがバッググラウンドに落としたアプリケーションの永続化

* 長時間の実行に耐えうる作りにする

* iOS or Androidでバックグラウンドアプリに必要とされているファイルの収集

上記を行うとされています（英語は色々とアレなので誤解があればご指摘ください）

