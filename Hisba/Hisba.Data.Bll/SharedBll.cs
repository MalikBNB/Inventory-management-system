using Hisba.Data.Layers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hisba.Data.Bll
{
    public class SharedBll
    {

        public static AppDbContext Db = new AppDbContext();
    }
}
