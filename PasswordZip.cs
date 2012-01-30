//
// ＜PasswordZip＞
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

using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace cockatiel.cage.net
{
	public class PasswordZip
	{
		//用例
		private static void Usage()
		{
			Console.WriteLine("usage:\n  PasswordZip <output> <input_file/dir> <input_file/dir2>... ");
			Environment.Exit(1);
		}

		//ファイルorディレクトリ存在フラグ
		private enum FATTR {
			NOT_EXIST = 0,
			FILE,
			DIRECTORY
		};

		//ファイル存在チェック
		private static FATTR IsExist(String item)
		{
			FATTR ret = FATTR.NOT_EXIST;
			if (System.IO.Directory.Exists(item))
			{
				ret = FATTR.DIRECTORY;
			}
			else if (System.IO.File.Exists(item))
			{
				ret = FATTR.FILE;
			}
			return (ret);
		}

		//展開
		private static void addItemsToZip(ref ZipFile zip, String[] args)
		{
			//入力ファイルを一つ一つ検査
			foreach (String item in args)
			{
				FATTR fattr;

				//存在しないファイルorディレクトリだったら次のアイテムへ
				if ((fattr = IsExist(item)) == FATTR.NOT_EXIST)
				{
					continue;
				}

				if (fattr == FATTR.FILE)
				{
					//ファイルの場合はそのまま追加
					Console.WriteLine("\tadding...  [file] {0}", item);
					zip.AddFile(item, "");
				}
				else if (fattr == FATTR.DIRECTORY)
				{
					//ディレクトリの場合は階層構造を保った上で追加
					Console.WriteLine("\tadding...  [dir]  {0}", item);
					System.IO.FileInfo info = new System.IO.FileInfo(item);
					zip.AddDirectory(item, info.Name);
				}
			}
		}

		//zipファイル名取得
		private static String GetZipFilename(String src)
		{
			Regex re = new Regex("\\.\\w+$");
			String dst = re.Replace(src,".zip");
			if (dst == src)
			{
				dst = src + ".zip";
			}
			return dst; 		   
		}

		//パスワード入力
		private static String GetPassword(String prompt)
		{
			Console.Write(prompt + ": ");
			String password = "";
			while (true)
			{
				ConsoleKeyInfo input = Console.ReadKey(true);
				if (input.Key == ConsoleKey.Enter)
				{
					Console.WriteLine();
					break;
				}
				else if (input.Key == ConsoleKey.Backspace)
				{
					if (password.Length > 0)
					{
						password.Substring(0, password.Length - 1);
					}
				}
				else
				{
					password += input.KeyChar;
				}
			}

			return password;
		}

		//main
		public static void Main(String[] args)
		{
			//入力ファイルがひとつもなかったら終了
			if (args.Length == 0) Usage();

			//出力ファイル名を取得
			String dst = GetZipFilename(args[0]);
			try
			{
				ZipFile zip = new ZipFile();

				//日本語ファイル名対策（for win）
				zip.ProvisionalAlternateEncoding = System.Text.Encoding.GetEncoding("shift_jis");
				//圧縮レベルを変更 
				zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
				//必要な時はZIP64で圧縮する。デフォルトはNever。 
				zip.UseZip64WhenSaving = Ionic.Zip.Zip64Option.AsNecessary;
				//エラーが出てもスキップする。デフォルトはThrow。 
				zip.ZipErrorAction = Ionic.Zip.ZipErrorAction.Skip;
				//コメント
                zip.Comment = "（´-`）.｡oO（This application is using DotNetZip library）\n";
                zip.Comment += "（´-`）.｡oO（...producted by K.Tsunezumi）\n";
                Console.Write(zip.Comment);

                //パスワード入力
                int retry;
                String pass = "", pass2;
                for (retry = 0; retry < 3; retry++)
                {
                    pass = GetPassword("Enter Password");
                    pass2 = GetPassword("Retry Password");
                    //3回間違えたらプログラムを終了
                    if (pass == pass2) break;
                }
               
                //パスワードをかけて暗号化（パスワードが空文字列の場合は暗号化しない）
				if (pass != "")
				{
					zip.Password = pass;
					zip.Encryption = Ionic.Zip.EncryptionAlgorithm.PkzipWeak;
                    Console.Write("（・∀・）PW encrypt is enabled!!\n");
                }

				//入力ファイル/ディレクトリを圧縮
				addItemsToZip(ref zip, args);

				//zipファイル保存
				zip.Save(dst);

				//オブジェクト削除
				zip.Dispose();


				Console.WriteLine("\toutput to >> " + dst);
                Console.Write("（・∀・）zip compressing is succeeded!!\n");
            }
			catch (System.Exception ex1)
			{
				//エラー
                System.Console.Error.WriteLine("（´・ω・`）error: " + ex1);
			}
            Console.Write("（・∀・）press any key");
			Console.ReadKey(true);
		}
	}
}