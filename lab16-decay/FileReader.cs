using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace lab16_decay
{
    static public class FileReader
    {
        static public List<string> ReadFile(string path)
        {
            if (!File.Exists(path)) throw new FileNotFoundException();
            try
            {
                List<string> lines = new List<string>();
                using(FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read))
                using(StreamReader reader = new StreamReader(stream))
                {
                    while(!reader.EndOfStream)
                    {
                        lines.Add(reader.ReadLine());
                    }
                }
                return lines;
            }catch (Exception ex)
            {
                throw new FileNotFoundException("error opening file.", ex);
            }
        }



    }
}
