using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RadioactiveDecayLib;
using JsonDTO;
using System.Net.Sockets;

namespace lab16_decay
{
    internal class Program
    {
        static Network conn;
        static Dictionary<int, Element> Elements;
        static void Main(string[] args)
        {
            Elements = GetDictionary("./Elements.txt");

            Student st = new Student(Console.ReadLine(), int.Parse(Console.ReadLine()));
            conn = new Network("127.0.0.1", 2048);
            conn.Send(st);

            BeginLoop();
            conn.Disconnect();
            Console.WriteLine("disconnected from server");
            Console.ReadLine();
        }

        private static void BeginLoop()
        {
            Element e1 = conn.Read<Element>();
            while (true)
            {
                object obj = conn.Read();
                Console.WriteLine(e1.ToString());
                if (obj is Element)
                {
                    Console.WriteLine("ELEMENT -----------------------");
                    e1 = (Element)obj;
                    continue;
                }
                else if (obj is Response)
                {
                    Console.WriteLine("RESPONSE -----------------------");
                    DecayEdit(ref e1, obj);
                    conn.Send(e1);
                }
                else if (obj is Result)
                {
                    Console.WriteLine("RESULT -----------------------");
                    Result result = (Result)obj;
                    Console.WriteLine(result.Status);
                    break;
                }
            }
        }

        private static void DecayEdit(ref Element element, object obj)
        {
            DecayType decay = ((Response)obj).Decay;
            if (decay == DecayType.Alpha)
            {
                //-2 atomic number
                // -4 mass (-2 nutrons and protons)
                element.AtomicNumber -= 2;
                element.MassNumber -= 4;
            }
            else//beta
            {
                //+1 atoamic number
                //mass stays the same
                element.AtomicNumber += 1;
            }
            element.Symbol = Elements[element.AtomicNumber].Symbol;
        }

        static private Dictionary<int, Element> GetDictionary(string FilePath)
        {
            Dictionary<int, Element> e = new Dictionary<int, Element>();
            foreach(string s in FileReader.ReadFile(FilePath))
            {
                string[] p = s.Split(' ');
                Element eElement = new Element(p[1], (int)double.Parse(p[2]),int.Parse(p[0]));
                e.Add(eElement.AtomicNumber, eElement);
            }
            return e;
        }
    }
}
