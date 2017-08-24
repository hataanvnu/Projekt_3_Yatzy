using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProtocolUtils
{
    public class PointField
    {
        public string Point { get; set; }
        public bool IsUsed { get; set; }

        public PointField()
        {
            Point = "-";
            IsUsed = false;
        }
    }


}
