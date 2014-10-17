using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace tailDotNet.Watchers
{
    public interface IStreamReader
    {
        string ReadLine();
        long Length { get; }
        long Position { get; }
    }
}
