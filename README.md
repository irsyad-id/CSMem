# CSMem

Create External Game Hacking Using C#.
Easy to use.

# Feature
- Read Memory (Support Module as BaseAddress)
- Write Memory (Support Module as BaseAddress)
- Pattern Scan ( Support Wildcard )
- Multi Level Pointer Support

# Usage
```
using CSMem;


namespace CSMemTest
{
    class Program
    {

        static void Main(string[] args)
        {
            var a = new CSMem.Manager("GameName");
            string input = "";
            do
            {
                input = Console.ReadLine();

                if(input.ToLower().Contains("nocd"))
                {
                    //Get Multiple Pointer Address
                    IntPtr Address = a.GetPatternAddress("UserAssembly.dll", "F3 0F 58 7B 70");
                    
                    //Write Byte To Address
                    a.WriteVirtualMemory(Address, new byte[] { 0x90,0x90,0x90,0x90,0x90});

                    List<int> NoCDOffset = new List<int>() { 0xA0, 0x20, 0x188, 0x110, 0x1D8, 0x28, 0x70 };
                    a.WriteVirtualMemory<float>("Game.dll", 0x05E643F8, NoCDOffset, float.Parse(input.Split(' ')[1].Trim()));
                }

            } while (!input.Equals("Q") || !input.Equals("q"));

            //Get Address From Pattern
            IntPtr Address = a.GetPatternAddress("module.dll", "f3 0f 11 86 ? ? ? ? 76 1d f3 0f 5e c1 f3 0f 11 45 08");
            ////Read int from address
            var bb = a.ReadVirtualMemory<float>(Address);

            ////Read Array Of Byte
            int relativeOffset = 0;
            List<int> offsets = new List<int>() { 0x43, 0x44, 0x0, 0xA0 };
            var b = a.ReadVirtualMemory(Address + relativeOffset, offsets, 10);
            Console.ReadLine();
        }
    }
}

```

You can take a look for another function in library.

Thx to Akaion i make it base on his Jupiter Library and learn from his Library source.

Akaion/Jupiter : https://github.com/Akaion/Jupiter

