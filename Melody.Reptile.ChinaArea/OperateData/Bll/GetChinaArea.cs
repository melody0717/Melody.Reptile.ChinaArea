using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using HtmlAgilityPack;
namespace Melody.Reptile.ChinaArea.OperateData.Bll
{
   public class GetChinaArea
    {
        string _indexUrl= "http://www.stats.gov.cn/tjsj/tjbz/tjyqhdmhcxhfdm/2015/index.html";

       

        public GetChinaArea()
        {
           
        }

        public List<Model.Map> GetMaps()
        {
            List<Model.Map> maps = new List<Model.Map>();
            List<Model.Map> provinces = GetProvinces();
            foreach (var map in provinces)
            {
#warning Need Delete
                Console.WriteLine(JoinStr(map.Depth) + "代码：" + map.Code + ",区域名称:" + map.Name);
                GetArea(_indexUrl.Replace("index.html", map.Code.Substring(0, 2) + ".html"), 1, map.Code, ref maps);
            }
            maps.AddRange(provinces);
            return maps;
        }

        public List<Model.Map> GetProvinces()
        {
            HtmlWeb web=new HtmlWeb();
            web.OverrideEncoding = System.Text.Encoding.GetEncoding("GB2312");
            HtmlDocument document = web.Load(_indexUrl);
            document.OptionReadEncoding = true;
            HtmlNodeCollection hrefList = document.DocumentNode.SelectNodes(".//a[@href]");
            List<Model.Map> results =new List<Model.Map>();

            if (hrefList == null)
            {
                return results;

            }

            foreach (HtmlNode href in hrefList)
            {
                Model.Map map=new Model.Map();

                HtmlAttribute att = href.Attributes["href"];

                if (att.Value.Contains("miibeian")) //去除备案
                {
                    continue;
                }

                map.Depth = 1;
                map.Code = att.Value.Replace(".html", "").PadRight(12, '0');
                map.Name = href.InnerText;
                map.ParentCode = "0";
                results.Add(map);
            }
            return results;
        }

        /// <summary>
        /// 递归读取方法
        /// </summary>
        /// <param name="url">当前深度地址</param>
        /// <param name="depth">上级深度</param>
        /// <param name="parentCode">上级代码</param>
        private void GetArea(string url, int depth, string parentCode,ref List<Model.Map> maps)
        {

            //递归读取方法
            HtmlWeb hw = new HtmlWeb();
            hw.OverrideEncoding = System.Text.Encoding.GetEncoding("GB2312");
            HtmlDocument doc = hw.Load(url);//是你需要解析的url
            doc.OptionReadEncoding = true;

            depth++;
            StringBuilder sb = new StringBuilder();
            HtmlNodeCollection hrefList = doc.DocumentNode.SelectNodes(".//tr[@class='" + GetClassName(depth) + "']/td[last()]");

            if (hrefList == null)
            {
                return;
            }

            foreach (HtmlNode href in hrefList)
            {
                Model.Map map=new Model.Map();

                HtmlNodeCollection hrefNode = href.SelectNodes(".//a[@href]");

                if (hrefNode != null)//链接存在
                {
                    HtmlAttribute att = hrefNode[0].Attributes["href"];

                    if (att.Value.Contains("miibeian")) //去除备案
                    {
                        continue;
                    }

                    string codeNum = href.PreviousSibling.InnerText.PadRight(12, '0');
                    maps.Add(new Model.Map()
                    {
                        Depth = depth,
                        Code = codeNum,
                        Name = href.InnerText,
                        ParentCode = parentCode
                    });
#warning Need Delete
                    Console.WriteLine(JoinStr(depth) + "代码：" +codeNum+",区域名称:"+href.InnerText);

                    string depurl = url.Replace(parentCode.Substring(0, 2 * (depth - 1)) + ".html", att.Value);
                    if (depth < 5)//到乡镇即可
                        GetArea(depurl, depth, codeNum,ref maps);
                }
                else
                {
                    if (depth == 5)
                    {
                        maps.Add(new Model.Map()
                        {
                            Depth = depth,
                            Code = href.PreviousSibling.PreviousSibling.InnerText.PadRight(12, '0'),
                            Name = href.InnerText,
                            ParentCode = parentCode
                        });
#warning Need Delete
                        Console.WriteLine(JoinStr(depth) + "代码：" + href.PreviousSibling.PreviousSibling.InnerText.PadRight(12, '0') +",区域名称:" + href.InnerText);
                    }
                    else
                    {
                       
                        maps.Add(new Model.Map()
                        {
                            Depth = depth,
                            Code = href.PreviousSibling.InnerText.PadRight(12, '0'),
                            Name = href.InnerText,
                            ParentCode = parentCode
                        });
#warning Need Delete
                        Console.WriteLine(JoinStr(depth) + "代码：" + href.PreviousSibling.InnerText.PadRight(12, '0')+ ",区域名称:" + href.InnerText);
                    }
                  
                }
            }
        }

        /// <summary>
        /// 不同级别不同的class名称
        /// </summary>
        /// <param name="depth">深度</param>
        /// <returns></returns>
        private string GetClassName(int depth)
        {
            string result = "";

            switch (depth)
            {
                case 2:
                    result = "citytr";
                    break;
                case 3:
                    result = "countytr";
                    break;
                case 4:
                    result = "towntr";
                    break;
                case 5:
                    result = "villagetr";
                    break;
                default:
                    break;
            };

            return result;
        }

#warning Need Delete
        /// <summary>
        /// 分级连接符
        /// </summary>
        /// <param name="depth"></param>
        /// <returns></returns>
        private string JoinStr(int depth)
        {
            string str = "";
            for (int i = 1; i < depth; i++)
            {
                str += "-----|";
            }
            return str;

        }
    }
}
