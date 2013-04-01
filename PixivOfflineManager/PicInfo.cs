﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using CsQuery;
using System.Text.RegularExpressions;

namespace PixivOfflineManager
{
	public enum InfoStatus { Waiting, Loading, Ready, Error };
	public class PicInfo
	{
		public string Dir;
		public string Id;
		public string[] Tags;
		public string Title;
		public string Artist;
		public string Url;
		public string Description;
		public InfoStatus Status;
		public Exception ErrorInfo;
		public PicInfo(string id, string dir)
		{
			Id = id;
			Dir = dir;
			Status = InfoStatus.Waiting;
		}
		public async void GetInfo()
		{
			try
			{
				Status = InfoStatus.Loading;
				CQ dom;
				WebRequest req = WebRequest.Create("http://www.pixiv.net/member_illust.php?mode=medium&illust_id=" + Id);
				using (WebResponse result = await req.GetResponseAsync())
				{
					using (Stream receviceStream = result.GetResponseStream())
					{
						using (StreamReader readerOfStream = new StreamReader(receviceStream, System.Text.Encoding.GetEncoding("utf-8")))
						{
							dom = readerOfStream.ReadToEnd();
						}
					}
				}
//<meta property="og:title" content="まこぴ | 否 [pixiv]">
				Title = dom["meta[property='og:title']"].Attr("content");
				Title = Title.Substring(0, new Regex("\\s|\\s").Match(Title).Index);
				//Tags = dom["meta[name='keywords']"].Attr<string>("content").Split(',');
				CQ CTags = dom["div.pedia>ul:first>li>a"];
				Tags = new string[CTags.Length];
				for(int i=0;i<Tags.Length;i++)
				{
					Tags[i] = new CQ(CTags[i]).Text();
				}
				CQ art = dom["div.front-subContent>h2>a:eq(1)"];
				Artist = art.Text();
				Url = "http://www.pixiv.net/" + art.Attr("href");
				Description = dom["meta[property='og:description']"].Attr("content");
#if DEBUG
				DebugInfo();
#endif
				Status = InfoStatus.Ready;
			}
			catch (Exception e)
			{
				Status = InfoStatus.Error;
				ErrorInfo = e;
			}
		}
#if DEBUG
		void DebugInfo()
		{
			Console.WriteLine("Title={0}\nArtist={1}\nUrl={2}\nDescription={3}", Title, Artist, Url, Description);
			Console.Write("Tags {0}", Tags[0]);
			for (int i = 1; i < Tags.Length; i++)
			{
				Console.Write(",{0}", Tags[i]);
			}
		}
#endif
	}
}
