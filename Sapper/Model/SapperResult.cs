using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sapper.Model
{
    public class SapperResult<TOutput>
    {
        public List<TOutput> Data { get; set; }
        public dynamic OutputParams { get; set; }
        public int ModifiedCount { get; set; }
    }

    public class SapperResult
    {
        public dynamic OutputParams { get; set; }
        public int ModifiedCount { get; set; }
    }
}
