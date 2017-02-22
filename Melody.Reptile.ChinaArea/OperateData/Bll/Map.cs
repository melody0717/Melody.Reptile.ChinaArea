using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melody.Reptile.ChinaArea.OperateData.Bll
{
   public class Map
    {
        public int Insert(List<Model.Map> maps)
        {
            OperateData.Dal.Map map=new Dal.Map();
            return  map.Insert(maps);
        }
    }
}
