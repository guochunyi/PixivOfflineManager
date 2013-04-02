using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Text.RegularExpressions;

namespace PixivOfflineManager
{
	class FileManager
	{
		static Regex fileRex = new Regex("([0-9]+)(_.*)*((\\.jpg)|(\\.png))");
		public List<PicInfo> Files;
		public FileManager(string path)
		{
			Files = new List<PicInfo>();
			string id;
			DirectoryInfo lib = new DirectoryInfo(path);
			foreach (FileInfo pic in lib.GetFiles())
			{
				if (fileRex.IsMatch(pic.Name))
				{
					id = fileRex.Replace(pic.Name, "$1");
					Files.Add(new PicInfo(id, pic.FullName));
					//Console.WriteLine("{0}=={1}", id, pic.FullName);
				}
			}
		}
	}
}
