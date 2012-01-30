//
// ＜PasswordZip＞ Ver.1.0.0.1
// 
// このプログラムはDotNetZipライブラリを使用して作成たオープンソースライセンスです。。
// ライセンスはDotNetZip dllのライセンスに準拠します（Microsoft Public License (Ms-PL)）。
// （ライブラリの取得元：http://dotnetzip.codeplex.com/）
// ライセンスの詳細はLisence.txtをご覧ください。
// 
// プログラムの著作権はライセンスとは関係なくK.Tsunezumiに属します。
// このプログラムを使用したことによるあらゆる損害に対しての責任は負いかねます。
// 個人の責任においてご使用をお願いします。
//                     

　
＜特徴＞
　パスワードつき暗号化zipファイルを生成するだけのツールです。
　Windows 7 でパスワードつき暗号化zipファイルを生成する機能がなくなってしまったので作成しました。
　zipファイルの展開（解凍）機能はありません。Windowsのzip展開機能や巷のツールを用いて解凍してください。
　
＜使い方＞
　コマンドプロンプト上で動作するアプリケーションです。GUIはありません。
　以下のファイルを適当なフォルダにコピーして後述の通りに使用してください。
　　・PasswordZip.exe
　　・Ionic.Zip.dll
　　・DotNetZip_License.txt ※ライブラリのライセンス許諾書
　　・DotNetZip_Readme.txt ※ライブラリのReadme
　　・Readme.txt ※このファイル
　　
　使い方はいろいろ。
　（その１）exeにzip圧縮したいファイル（複数可）をドラッグアンドドロップする
　（その２）『送る』フォルダにexeのショートカットを作成し、エクスプローラから『送る』で追加したショートカットを指定する。
　（その３）地道にコマンドプロンプトで実行する。
　　　例: PasswordZip.exe output input1 input2 ... 
　　　　output.zipという圧縮ファイルが生成されます

　パスワードは必ず聞いてきます。
　暗号化を擦る必要がないのであれば何も入力せずにリターンキーを押下してください。　
　
　また、ツールを初めて使う際に『.net framework 2.0のライブラリがない』などの実行時エラーが発生する場合があります。
　Microsoftのサイトよりランタイムをインストールしてから試してみてください。
　http://www.microsoft.com/downloads/ja-jp/details.aspx?FamilyID=0856eacb-4362-4b0d-8edd-aab15c5e04f5

＜ソースファイル＞
　https://github.com/kiyotune/passwordzip_for_win7
　Visual Studio 2005(C#) でビルド＆確認しています。
　
＜その他＞
　突貫アプリなのでもしかしたら不具合があるかもしれません。勝手に直してください。
　ライセンス条項で不備がありましたら是非お知らせください。
　再頒布してもかまいませんがプログラムのソースコードの著作権はK.Tsunezumiに存在します。
　ライセンスや法律を遵守した上での運用をお願いします。
　その他不備がありましたらお知らせください。
　

2011/6/21 K.Tsunezumi (kiyotune@cockatiel-cage.net) add