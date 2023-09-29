using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS_Test.Define
{
    interface IBMS_Board
    {
        string Name { get; set; }
        string ID { get; set; }
        string SN_Code { get; set; }
        int Capacity { get; set; }
    }
}
