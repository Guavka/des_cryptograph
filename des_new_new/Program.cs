using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace des_new_new
{
    class Program
    {
        static byte[] strBytes = new byte[8];
        static byte[] key_byte = new byte[8];
        static BitArray block_A = new BitArray(32);
        static BitArray block_B = new BitArray(32);
        static BitArray key_c = new BitArray(28);
        static BitArray key_d = new BitArray(28);
        static BitArray strBytes_bit;
        static BitArray key_bit;
        static BitArray[] all_key=new BitArray[16];
        static BitArray ep_block;

        static public void etap1(string str)
        {
            byte[] strBytes1 = Encoding.ASCII.GetBytes(str);

            init_arr(strBytes, strBytes1);
            strBytes_bit =new BitArray(strBytes);
            Console.WriteLine("start message");
            for (int i = 0; i < strBytes_bit.Length; i++)
            {

                if ((i == 7) || (i == 15) || (i == 23) || (i == 31) || (i == 39) || (i == 47) || (i == 55) || (i == 63))
                    Console.WriteLine("{0,5} ",strBytes_bit[i]);
                else
                    Console.Write("{0,7} ", strBytes_bit[i]);
            }
            strBytes_bit = white_arr(strBytes_bit);
            blocks_make(strBytes_bit, block_A, block_B);
        }
        static public BitArray white_arr(BitArray strBytes)
        {
            BitArray temp = new BitArray(64);
            for (int i = 0, k = 57; i < temp.Length; i++, k -= 8)
            {
                if (k < 0)
                {
                    switch (i)
                    {
                        case 8:
                            k = 59; break;
                        case 16:
                            k = 61; break;
                        case 24:
                            k = 63; break;
                        case 32:
                            k = 56; break;
                        case 40:
                            k = 58; break;
                        case 48:
                            k = 60; break;
                        case 56:
                            k = 62; break;
                        default:
                            k = 57;
                            break;
                    }
                    temp[i] = strBytes[k];
                }
                temp[i] = strBytes[k];
            }
            return temp;
        }
        static public void blocks_make(BitArray strBytes, BitArray block1, BitArray block2)
        {
            for (int i = 0; i < strBytes.Length / 2; i++)
                block1[i] = strBytes[i];
            for (int i = strBytes.Length / 2, k = 0; i < strBytes.Length; i++, k++)
                block2[k] = strBytes[i];
        }
        static public byte[] init_arr(byte[] strBytes, byte[] strBytes1)
        {
            for (int i = 0; i < strBytes1.Length; i++)
                strBytes[i] = strBytes1[i];
            return strBytes;
        }

        static public void etap_key(string key)
        {
            byte[] key_byte1 = Encoding.ASCII.GetBytes(key);
            init_arr(key_byte, key_byte1);
            key_bit = new BitArray(key_byte);
            key_blocks_make(key_bit, key_c, key_d);
            init_key(all_key);
            for (int i = 1; i <= 16; i++)
                key_iteration(key_c, key_d, i);

        }
        static public void init_key(BitArray[] all_key)
        {
            for (int i = 0; i < all_key.Length; i++)
            {
                all_key[i] = new BitArray(48);
            }
        }
        static public void key_blocks_make(BitArray key_bit, BitArray key_c, BitArray key_d)
        {
            for (int i = 0, k = 56; i < key_c.Length; i++, k -= 8)
            {
                if (k < 0)
                {
                    switch (i)
                    {
                        case 8:
                            k = 57; break;
                        case 16:
                            k = 58; break;
                        case 24:
                            k = 59; break;

                        default:
                            k = 57;
                            break;
                    }
                    key_c[i] = key_bit[k];
                }
                key_c[i] = key_bit[k];
            }
            for (int i = 0, k = 62; i < key_d.Length; i++, k -= 8)
            {
                if (k < 0)
                {
                    switch (i)
                    {
                        case 8:
                            k = 61; break;
                        case 16:
                            k = 60; break;
                        case 24:
                            k = 27; break;

                        default:
                            k = 62;
                            break;
                    }
                    key_d[i] = key_bit[k];
                }
                key_d[i] = key_bit[k];
            }

        }
        static public void key_iteration(BitArray key_c, BitArray key_d, int round)
        {
            int n;
            
            if ((round == 1) || (round == 2) || (round == 9) || (round == 16))
                n = 1;
            else
                n = 2;
            key_sdv(key_c,n);
            key_sdv(key_d,n);
            all_key[round - 1] = CP_keys(key_c, key_d);
        }
        static public void key_sdv(BitArray key,int n)
        {
            int len = key.Length; //длина массива бит
            bool c = key[0]; //младший бит

            //создание массива типа bool 
            //такой же длины как gamma_value типа BitArray
            bool[] gamma_new = new bool[len];

            //копируем массив типа BitArray в массив типа bool
            key.CopyTo(gamma_new, 0);

            //копируем массив bool со сдвигом на 1ну позицию в другой массив bool
            Array.Copy(gamma_new, 1, gamma_new, 0, len - 1);            

            //замена последнего элемента массива bool
            gamma_new[len - 1] = c;

            //преобразование массива типа bool в массив типа BitArray
            key = new BitArray(gamma_new); 
        }
        static public BitArray CP_keys(BitArray key_c, BitArray key_d)
        {
            BitArray test = new BitArray(56);
            BitArray test1 = new BitArray(48);
            int[] CP_table = { 14, 17, 11, 24, 1, 5, 3, 28, 15, 6, 21, 10, 23, 19, 12, 4, 26, 8, 16, 7, 27, 20, 13, 2, 41, 52, 31, 37, 47, 55, 30, 40, 51, 45, 33, 48, 44, 49, 39, 56, 34, 53, 46, 42, 50, 36, 29, 32 };

            for (int i = 0; i < test.Length / 2; i++)
                test[i] = key_c[i];
            for (int i = test.Length / 2, k = 0; i < test.Length; i++, k++)
                test[i] = key_d[k];

            for (int i = 0; i < test1.Length; i++)
                test1[i] = test[CP_table[i] - 1];
            return test1;
        }

        static public BitArray function_f(BitArray need_block, int key)
        {
            BitArray s1 = new BitArray(6);
            BitArray s2 = new BitArray(6);
            BitArray s3 = new BitArray(6);
            BitArray s4 = new BitArray(6);
            BitArray s5 = new BitArray(6);
            BitArray s6 = new BitArray(6);
            BitArray s7 = new BitArray(6);
            BitArray s8 = new BitArray(6);
            int[,] tabl1 = { 
                               {14,4,13,1,2,15,11,8,3,10,6,12,5,9,0,7},
                               {0,15,7,4,14,2,13,1,10,6,12,11,9,5,3,8},
                               {4,1,14,8,13,6,2,11,15,12,9,7,3,10,5,0},
                               {15,12,8,2,4,9,1,7,5,11,3,14,10,0,6,13}
                           };
            int[,] tabl2 = { 
                               {15,1,8,14,6,11,3,4,9,7,2,13,12,0,5,10},
                               {3,13,4,7,15,2,8,14,12,0,1,10,6,9,11,5},
                               {0,14,7,11,10,4,13,1,5,8,12,6,9,3,2,15},
                               {13,8,10,1,3,15,4,2,11,6,7,12,0,5,14,9}
                           };
            int[,] tabl3 = { 
                               {10,0,9,14,6,3,15,5,1,13,12,7,11,4,2,8},
                               {13,7,0,9,3,4,6,10,2,8,5,14,12,11,15,1},
                               {13,6,4,9,8,15,3,0,11,1,2,12,5,10,14,7},
                               {1,10,13,0,6,9,8,7,4,15,14,3,11,5,2,12}
                           };
            int[,] tabl4 = { 
                               {7,13,14,3,0,6,9,10,1,2,8,5,11,12,4,15},
                               {13,8,11,5,6,15,0,3,4,7,2,12,1,10,14,9},
                               {10,6,9,0,12,11,7,13,15,1,3,14,5,2,8,4},
                               {3,15,0,6,10,1,13,8,9,4,5,11,12,7,2,14}
                           };
            int[,] tabl5 = { 
                               {2,12,4,1,7,10,11,6,5,5,3,15,13,0,14,9},
                               {14,11,2,12,4,7,13,1,5,0,15,10,3,9,8,6},
                               {4,2,1,11,10,13,7,8,15,9,12,5,6,3,0,14},
                               {11,8,12,7,1,14,2,13,6,15,0,9,10,4,5,3}
                           };
            int[,] tabl6 = { 
                               {12,1,10,15,9,2,6,8,0,13,3,4,14,7,5,11},
                               {10,15,4,2,7,12,9,5,6,1,13,14,0,11,3,8},
                               {9,14,15,5,2,8,12,3,7,0,4,10,1,13,11,6},
                               {4,3,2,12,9,5,15,10,11,14,1,7,6,0,8,3}
                           };
            int[,] tabl7 = { 
                               {4,11,2,14,15,0,8,13,3,12,9,7,5,10,6,1},
                               {13,0,11,7,4,9,1,10,14,3,5,12,2,15,8,6},
                               {1,4,11,13,12,3,7,14,10,15,6,8,0,5,9,2},
                               {6,11,13,8,1,4,10,7,9,5,0,15,14,2,3,12}
                           };
            int[,] tabl8 = { 
                               {13,2,8,4,6,15,11,1,10,9,3,14,5,0,12,7},
                               {1,15,13,8,10,3,7,4,12,5,6,11,0,14,9,2},
                               {7,11,4,1,9,12,14,2,0,6,10,13,15,3,5,8},
                               {2,1,14,7,4,10,8,13,15,12,9,0,3,5,6,11}
                           };
            int[] P = {16,7,20,21,29,12,28,17,1,15,23,26,5,18,31,10,2,8,24,14,32,27,3,9,19,13,30,6,22,11,4,25};

            BitArray s_table_block = new BitArray(32);
            BitArray s_table_block1 = new BitArray(32);
            BitArray temp_block = need_block;
            ep_block = EP(temp_block);
            ep_block.Xor(all_key[key]);

            for (int i = 0; i < s1.Length; i++)
            {
                s1[i] = ep_block[i];
                s2[i] = ep_block[i + 6];
                s3[i] = ep_block[i + 12];
                s4[i] = ep_block[i + 18];
                s5[i] = ep_block[i + 24];
                s6[i] = ep_block[i + 30];
                s7[i] = ep_block[i + 36];
                s8[i] = ep_block[i + 42];
            }
            s1 = s_parse(s1, tabl1);
            s2 = s_parse(s2, tabl2);
            s3 = s_parse(s3, tabl3);
            s4 = s_parse(s4, tabl4);
            s5 = s_parse(s5, tabl5);
            s6 = s_parse(s6, tabl6);
            s7 = s_parse(s7, tabl7);
            s8 = s_parse(s8, tabl8);
            
            for (int i = 0,k=0; i < s_table_block.Length; i++,k++)
            {
                if (i < 4)
                    s_table_block[i] = s1[k];
                else
                {
                    k = 0;
                    if (i < 8)
                        s_table_block[i] = s2[k];
                    else
                    {
                        k = 0;
                        if (i < 12)
                            s_table_block[i] = s3[k];
                        else
                        {
                            k = 0;
                            if (i < 16)
                                s_table_block[i] = s4[k];
                            else
                            {
                                k = 0;
                                if (i < 20)
                                    s_table_block[i] = s5[k];
                                else
                                {
                                    k = 0;
                                    if (i < 24)
                                        s_table_block[i] = s6[k];
                                    else
                                    {
                                        k = 0;
                                        if (i < 28)
                                            s_table_block[i] = s7[k];
                                        else
                                        {
                                            k = 0;
                                            s_table_block[i] = s8[k];
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            for (int i = 0; i < s_table_block.Length; i++)
                s_table_block1[i] = s_table_block[P[i] - 1];

            return s_table_block1;                                 
      }
        static public BitArray s_parse(BitArray s1,int[,] tabl)
        {
            
            int row = Convert.ToInt32(s1[0]) * (int)Math.Pow(2, 0) + Convert.ToInt32(s1[1]) * (int)Math.Pow(2, 1);
            int col = Convert.ToInt32(s1[2]) * (int)Math.Pow(2, 0) + Convert.ToInt32(s1[3]) * (int)Math.Pow(2, 1) + Convert.ToInt32(s1[4]) * (int)Math.Pow(2, 2) + Convert.ToInt32(s1[5]) * (int)Math.Pow(2, 3);
            int index = tabl[row, col];
            BitArray b = new BitArray(new byte[] { Convert.ToByte(index) });
            BitArray test = new BitArray(4);
            test[3] = b[0];
            test[2] = b[1];
            test[1] = b[2];
            test[0] = b[3];
            return test;
        }
        
        static public BitArray EP(BitArray block)
        {
            bool flag = false;
            BitArray temp = new BitArray(48);
            temp[1] = block[0];
            temp[temp.Length - 1] = block[0];

            temp[temp.Length - 2] = block[block.Length - 1];
            temp[0] = block[block.Length - 1];
            for (int i = 1, k = 0, j = 2; i < block.Length - 1; i++, k++, j++)
            {
                if (k == 2)
                {
                    flag = true;
                }
                if (k == 4)
                {
                    flag = false;
                    k = 0;
                    j += 2;
                }
                if (!flag)
                    temp[j] = block[i];
                else
                {
                    temp[j] = block[i];
                    temp[j + 2] = block[i];
                }

            }
            return temp;
        }

        static public void swap()
        {
            BitArray temp = new BitArray(32);
            temp = block_A;
            block_A = block_B;
            block_B = temp;
        }
        static public void DES_alg(string str, string key)
        {
            
            BitArray final_message = new BitArray(64); 
            etap1(str);//2 блока по 32 бита
            etap_key(key);//все ключи

            for (int i = 0; i < 16; i++)
            {
                block_A = block_A.Xor(function_f(block_B, i));
                if (i!=15) swap();
            }
            final_message=final(block_A, block_B);
            Console.WriteLine();
            Console.WriteLine("final_message with DES");
            for (int i = 0; i < final_message.Length; i++)
            {
                
                if ((i==7)||(i==15)||(i==23)||(i==31)||(i==39)||(i==47)||(i==55)||(i==63))
                    Console.WriteLine("{0,5} ", final_message[i]);
                else
                    Console.Write("{0,7} ", final_message[i]);
            }
            
            
        }
        static public void DES_alg_obr(string str, string key)
        {
            BitArray final_message = new BitArray(64);
            etap1(str);//2 блока по 32 бита
            etap_key(key);//все ключи

            for (int i = 0,k=15; i < 16; i++,k--)
            {
                block_A = block_A.Xor(function_f(block_B, k));
                if (i != 15) swap();
            }
            final_message = final(block_A, block_B);         
        }
        static public BitArray final(BitArray a, BitArray b)
        {
            BitArray test = new BitArray(64);
            BitArray test1 = new BitArray(64); 
            int[] final_table = {40,8,48,16,56,24,64,32,39,7,47,15,55,23,63,31,38,6,46,14,54,22,62,30,37,5,45,13,53,21,61,29,36,4,44,12,52,20,60,28,35,3,43,11,51,19,59,27,34,2,42,10,50,18,58,26,33,1,41,9,49,17,57,25};

            for (int i = 0; i < test.Length / 2; i++)
                test[i] = a[i];
            for (int i = test.Length / 2, k = 0; i < test.Length; i++, k++)
                test[i] = b[k];

            for (int i = 0; i < test1.Length; i++)
                test1[i] = test[final_table[i] - 1];

            return test1;
        }
        static void Main(string[] args)
        {
            string str = "hello";
            string key = "world";            
            DES_alg(str, key);
            //DES_alg_obr(str, key);
            Console.ReadKey();  
        }
    }
}
