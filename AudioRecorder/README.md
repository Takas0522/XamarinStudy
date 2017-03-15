# 開発時メモ
参考
http://dev.classmethod.jp/smartphone/xamarin-ios-speechrecognizersample/

## API
録音機能はDependencyServiceを利用して、各OSのNativeAPIを使用します。
Androidは下記を使用
* Android.Media.AudioRecord
https://developer.xamarin.com/api/type/Android.Media.AudioRecord/

iOSは下記を使用
* AVAudioEngine
https://developer.xamarin.com/api/type/AVFoundation.AVAudioEngine/

あと、作成した音声ファイルは中身を確認したいので

PCLStorageで音声ファイルを格納したいと思います。

## APIの使い方
そもそも、AVAudioEngineってどう使うの？ってことですが、iOSでの使い方をググってみます。

http://qiita.com/ahirusun/items/2233c23a0e8070d265ea#avaudioengine%E3%81%AE%E5%9F%BA%E6%9C%AC%E7%9A%84%E3%81%AA%E4%BD%BF%E3%81%84%E6%96%B9

installTapOnBusで取得音声のCallBackが拾えるようです。
下記の通り、APIとしても準備されているようです。じゃあ、この形式でつくっていってみましょう。
https://developer.xamarin.com/api/member/AVFoundation.AVAudioNode.InstallTapOnBus/

## DependencyService
### Interfaceを作る
Formsから、Android、iOSで共通してよびだすためのInterfaceを作成します。

現状深く考えずにStartとStopだけ実装します。

あとで共通で必要そうなものやイベントハンドリングが出てきたら適宜追加していきます。

ひとまずは下記の感じですね。超単純です。

``` csharp
public interface IRecorder
{
    void RecStart();
    void RecEnd();
}
```

## iOSにInterfaceを実装
iOS側に実動作を実装します。上記で出てきたAVAudioEngineの使用を実装していくわけですね。

iOSや上記の参考サイトでは、実行前のstopがお約束のようなので、お約束に則って作ってみます。

``` csharp
NSError err;
public void RecStart()
{
    _audioEngine.Stop();
    _audioEngine.Prepare();
    _audioEngine.StartAndReturnError(out err);
}
```

output側はちとややこしいです。

BufferはAVAudioPcmBufferで出力されるので、PCLで取り回しをしやすくするためStreamかByteに変換したいです。

また、Bufferの変換部分等々を見てみると、単純にFunctionでByte[]返却するのは色々とややこしそうなので

https://forums.xamarin.com/discussion/59539/avaudioengine-tap-weird-values-strings-in-byte-array

イベントハンドラでハンドリングする方が良さげです、そのようにInterfaceを作成し直してみて、結果下記のようになりました。

``` csharp
public class RecordingEventArgs : EventArgs
{
    public byte[] RecordAudio { get; set; }
}
public delegate void RecordingEventHandler(object sender, RecordingEventArgs args);

public interface IRecorder
{
    void RecStart();
    void RecEnd();
    event RecordingEventHandler RecordingReceived;
}
```
``` csharp
public void RecEnd()
{
    var inputNode = _audioEngine.InputNode;
    var recordingFormat = inputNode.GetBusOutputFormat(0);
    inputNode.InstallTapOnBus(0, 1024, recordingFormat, (buffer, when) =>
    {
        if (buffer != null)
        {
            byte[] managedArray = new byte[buffer.FrameLength * 2];
            Marshal.Copy(buffer.Int16ChannelData, managedArray, 0, (int)buffer.FrameLength * 2);
            this.RecordingReceived(this, new RecordingEventArgs
            {
                RecordAudio = managedArray
            });
        }				
    });
}
```

## Forms実装

上記で作成したInterfaceをFormsで使用するよう変更していきます。

画面の方は適当にStartButtonとEncButtonを配置します。

``` csharp
public AudioRecorderPage()
{
    InitializeComponent();
    StartButton.Clicked += new EventHandler(StartButtonClicked);
    EndButton.Clicked += new EventHandler(EndButtonClicked);
}
private void StartButtonClicked(object sender, EventArgs e) {
    var recService = DependencyService.Get<IRecorder>();
    recService.RecordingReceived += async(_, args) =>
    {
        var rootFolder = FileSystem.Current.LocalStorage;
        var file = await rootFolder.CreateFileAsync("sample.wav", CreationCollisionOption.ReplaceExisting);
        var byteLength = args.RecordAudio.Length - 1;
        using (Stream stream = await file.OpenAsync(FileAccess.ReadAndWrite))
        {
            stream.Write(args.RecordAudio, 0, byteLength);
        }
    };
    recService.RecStart();
}
private void EndButtonClicked(object sender, EventArgs e) {
    var recService = DependencyService.Get<IRecorder>();
    recService.RecEnd();
}
```

https://bugzilla.xamarin.com/show_bug.cgi?id=38551

ここでハマり、結果としてXamarin公式のrecipeにたどり着く‥

結果として、ソースファイルの通りの内容となる。。。

Androidは下記を参考にした（手抜き）。
https://developer.xamarin.com/guides/xamarin-forms/web-services/cognitive-services/