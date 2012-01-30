//
// ��PasswordZip��
// 
// ���̃v���O������DotNetZip���C�u�������g�p���č쐬���I�[�v���\�[�X���C�Z���X�ł��B�B
// ���C�Z���X��DotNetZip dll�̃��C�Z���X�ɏ������܂��iMicrosoft Public License (Ms-PL)�j�B
// �i���C�u�����̎擾���Fhttp://dotnetzip.codeplex.com/�j
// ���C�Z���X�̏ڍׂ�Lisence.txt���������������B
// 
// �v���O�����̒��쌠�̓��C�Z���X�Ƃ͊֌W�Ȃ�K.Tsunezumi�ɑ����܂��B
// ���̃v���O�������g�p�������Ƃɂ�邠���鑹�Q�ɑ΂��Ă̐ӔC�͕������˂܂��B
// �l�̐ӔC�ɂ����Ă��g�p�����肢���܂��B
//					   

using System;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using Ionic.Zip;

namespace cockatiel.cage.net
{
	public class PasswordZip
	{
		//�p��
		private static void Usage()
		{
			Console.WriteLine("usage:\n  PasswordZip <output> <input_file/dir> <input_file/dir2>... ");
			Environment.Exit(1);
		}

		//�t�@�C��or�f�B���N�g�����݃t���O
		private enum FATTR {
			NOT_EXIST = 0,
			FILE,
			DIRECTORY
		};

		//�t�@�C�����݃`�F�b�N
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

		//�W�J
		private static void addItemsToZip(ref ZipFile zip, String[] args)
		{
			//���̓t�@�C����������
			foreach (String item in args)
			{
				FATTR fattr;

				//���݂��Ȃ��t�@�C��or�f�B���N�g���������玟�̃A�C�e����
				if ((fattr = IsExist(item)) == FATTR.NOT_EXIST)
				{
					continue;
				}

				if (fattr == FATTR.FILE)
				{
					//�t�@�C���̏ꍇ�͂��̂܂ܒǉ�
					Console.WriteLine("\tadding...  [file] {0}", item);
					zip.AddFile(item, "");
				}
				else if (fattr == FATTR.DIRECTORY)
				{
					//�f�B���N�g���̏ꍇ�͊K�w�\����ۂ�����Œǉ�
					Console.WriteLine("\tadding...  [dir]  {0}", item);
					System.IO.FileInfo info = new System.IO.FileInfo(item);
					zip.AddDirectory(item, info.Name);
				}
			}
		}

		//zip�t�@�C�����擾
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

		//�p�X���[�h����
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
			//���̓t�@�C�����ЂƂ��Ȃ�������I��
			if (args.Length == 0) Usage();

			//�o�̓t�@�C�������擾
			String dst = GetZipFilename(args[0]);
			try
			{
				ZipFile zip = new ZipFile();

				//���{��t�@�C�����΍�ifor win�j
				zip.ProvisionalAlternateEncoding = System.Text.Encoding.GetEncoding("shift_jis");
				//���k���x����ύX 
				zip.CompressionLevel = Ionic.Zlib.CompressionLevel.BestCompression;
				//�K�v�Ȏ���ZIP64�ň��k����B�f�t�H���g��Never�B 
				zip.UseZip64WhenSaving = Ionic.Zip.Zip64Option.AsNecessary;
				//�G���[���o�Ă��X�L�b�v����B�f�t�H���g��Throw�B 
				zip.ZipErrorAction = Ionic.Zip.ZipErrorAction.Skip;
				//�R�����g
                zip.Comment = "�i�L-`�j.�oO�iThis application is using DotNetZip library�j\n";
                zip.Comment += "�i�L-`�j.�oO�i...producted by K.Tsunezumi�j\n";
                Console.Write(zip.Comment);

                //�p�X���[�h����
                int retry;
                String pass = "", pass2;
                for (retry = 0; retry < 3; retry++)
                {
                    pass = GetPassword("Enter Password");
                    pass2 = GetPassword("Retry Password");
                    //3��ԈႦ����v���O�������I��
                    if (pass == pass2) break;
                }
               
                //�p�X���[�h�������ĈÍ����i�p�X���[�h���󕶎���̏ꍇ�͈Í������Ȃ��j
				if (pass != "")
				{
					zip.Password = pass;
					zip.Encryption = Ionic.Zip.EncryptionAlgorithm.PkzipWeak;
                    Console.Write("�i�E�́E�jPW encrypt is enabled!!\n");
                }

				//���̓t�@�C��/�f�B���N�g�������k
				addItemsToZip(ref zip, args);

				//zip�t�@�C���ۑ�
				zip.Save(dst);

				//�I�u�W�F�N�g�폜
				zip.Dispose();


				Console.WriteLine("\toutput to >> " + dst);
                Console.Write("�i�E�́E�jzip compressing is succeeded!!\n");
            }
			catch (System.Exception ex1)
			{
				//�G���[
                System.Console.Error.WriteLine("�i�L�E�ցE`�jerror: " + ex1);
			}
            Console.Write("�i�E�́E�jpress any key");
			Console.ReadKey(true);
		}
	}
}