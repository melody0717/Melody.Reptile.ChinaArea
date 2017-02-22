using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Melody.Reptile.ChinaArea.OperateData.Model
{
   public class Map
    {
        public string Code { get; set; }
        public string  Name { get; set; }
        public int Depth { get; set; }
        public string ParentCode { get; set; }
    }
}
