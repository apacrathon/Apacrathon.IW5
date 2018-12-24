using InfinityScript;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apacrathon
{
    // NoKnife plugin by zxz0O0 (Slightly modified to fit script).
    public class AntiKnife
    {
        private int ProcessID = System.Diagnostics.Process.GetCurrentProcess().Id;

        private int DefaultKnifeAddress;
        private unsafe int* KnifeRange;
        private unsafe int* ZeroAddress;

        public unsafe void SetupKnife()
        {
            Directory.CreateDirectory(AtlasiSnipe.d_script + @"\Knife");

            try
            {
                byte?[] search1 = new byte?[23]
                {
                  new byte?((byte) 139),
                  new byte?(),
                  new byte?(),
                  new byte?(),
                  new byte?((byte) 131),
                  new byte?(),
                  new byte?((byte) 4),
                  new byte?(),
                  new byte?((byte) 131),
                  new byte?(),
                  new byte?((byte) 12),
                  new byte?((byte) 217),
                  new byte?(),
                  new byte?(),
                  new byte?(),
                  new byte?((byte) 139),
                  new byte?(),
                  new byte?((byte) 217),
                  new byte?(),
                  new byte?(),
                  new byte?(),
                  new byte?((byte) 217),
                  new byte?((byte) 5)
                };
                KnifeRange = (int*)(FindMem(search1, 1, 4194304, 5242880) + search1.Length);
                if ((int)KnifeRange == search1.Length)
                {
                    byte?[] search2 = new byte?[25]
                    {
                        new byte?((byte) 139),
                        new byte?(),
                        new byte?(),
                        new byte?(),
                        new byte?((byte) 131),
                        new byte?(),
                        new byte?((byte) 24),
                        new byte?(),
                        new byte?((byte) 131),
                        new byte?(),
                        new byte?((byte) 12),
                        new byte?((byte) 217),
                        new byte?(),
                        new byte?(),
                        new byte?(),
                        new byte?((byte) 141),
                        new byte?(),
                        new byte?(),
                        new byte?(),
                        new byte?((byte) 217),
                        new byte?(),
                        new byte?(),
                        new byte?(),
                        new byte?((byte) 217),
                        new byte?((byte) 5)
                    };
                    this.KnifeRange = (int*)(FindMem(search2, 1, 4194304, 5242880) + search2.Length);
                    if ((int)this.KnifeRange == search2.Length)
                        this.KnifeRange = null;
                }
                this.DefaultKnifeAddress = *this.KnifeRange;
                byte?[] search3 = new byte?[24]
                {
                  new byte?((byte) 217),
                  new byte?((byte) 92),
                  new byte?(),
                  new byte?(),
                  new byte?((byte) 216),
                  new byte?(),
                  new byte?(),
                  new byte?((byte) 216),
                  new byte?(),
                  new byte?(),
                  new byte?((byte) 217),
                  new byte?((byte) 92),
                  new byte?(),
                  new byte?(),
                  new byte?((byte) 131),
                  new byte?(),
                  new byte?((byte) 1),
                  new byte?((byte) 15),
                  new byte?((byte) 134),
                  new byte?(),
                  new byte?((byte) 0),
                  new byte?((byte) 0),
                  new byte?((byte) 0),
                  new byte?((byte) 217)
                };

                ZeroAddress = (int*)(FindMem(search3, 1, 4194304, 5242880) + search3.Length + 2);
                
                if (!((int)KnifeRange != 0 && DefaultKnifeAddress != 0 && (int)ZeroAddress != 0))
                    Log.Error("Error finding address: NoKnife Plugin will not work");
            }
            catch (Exception ex)
            {
                Log.Error("Error in NoKnife Plugin. Plugin will not work.");
                Log.Error(ex.ToString());
            }

            //looks bad, but actually the else will always be fired "first"
            if (DefaultKnifeAddress == (int)ZeroAddress)
            {
                if (!File.Exists(AtlasiSnipe.d_script + @"\Knife\addr_" + ProcessID))
                {
                    //    print("now it will be feked");
                    Log.Error("Error: NoKnife will not work.");
                    return;
                }

                // print("restoring proper knife addr");

                DefaultKnifeAddress = int.Parse(File.ReadAllText(AtlasiSnipe.d_script + @"\Knife\addr_" + ProcessID));

                //  print("done");

            }
            else
            {
                File.WriteAllText(AtlasiSnipe.d_script + @"\Knife\addr_" + ProcessID, DefaultKnifeAddress.ToString());     //save for when it's feked
            }
        }

        public unsafe void DisableKnife()
        {
            *KnifeRange = (int)ZeroAddress;
        }

        public unsafe void EnableKnife()
        {
            *KnifeRange = DefaultKnifeAddress;
        }

        private unsafe int FindMem(byte?[] search, int num = 1, int start = 16777216, int end = 63963136)
        {
            try
            {
                int num1 = 0;
                for (int index1 = start; index1 < end; ++index1)
                {
                    byte* numPtr = (byte*)index1;
                    bool flag = false;
                    for (int index2 = 0; index2 < search.Length; ++index2)
                    {
                        if (search[index2].HasValue)
                        {
                            int num2 = *numPtr;
                            byte? nullable = search[index2];
                            if ((num2 != nullable.GetValueOrDefault() ? 1 : (!nullable.HasValue ? 1 : 0)) != 0)
                                break;
                        }
                        if (index2 == search.Length - 1)
                        {
                            if (num == 1)
                            {
                                flag = true;
                            }
                            else
                            {
                                ++num1;
                                if (num1 == num)
                                    flag = true;
                            }
                        }
                        else
                            ++numPtr;
                    }
                    if (flag)
                        return index1;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error in FindMem:" + ex.Message);
            }
            return 0;
        }
    }
}
