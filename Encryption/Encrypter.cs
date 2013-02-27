using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//referenced
using System.IO; //StreamReader , StreamWriter

namespace Encryption
{
    class Encrypter
    {
        private byte[] EncryptData(byte[] text, String key)
        {
            //Creates a character array for the result (should not hit above 4096)
            byte[] result = new byte[text.Length];

            //Doubt it matters that much, but for the speed!
            int keyLength = key.Length-1;

            //Perform XOR encryption rotating the key
            for (int i = 0, keyCounter = 0; i < text.Length; i++, keyCounter++)
            {
                //reaching end of the key, reset the index to beginning
                if (keyCounter > keyLength)
                {
                    keyCounter = 0;
                }
                //XOR  the data
                result[i] = (byte) (text[i] ^ key[keyCounter]);
            }
            
            //return the bytes
            return result;
        }

        public void EncryptFile(String fileName, String key, String outputName)
        {
            /*
             * Using FileStream now because StreamReader was not outputting the data properly
             * Files need to be read and written using BinaryReader in order to properly preserve the data as well as ensure proper encryption
             */
            FileStream inFile = new FileStream(@fileName, FileMode.Open, FileAccess.Read);
            BinaryReader inRead = new BinaryReader(inFile);

            //Delete existing files
            if (File.Exists(@outputName))
            {
                File.Delete(@outputName);
            }

            //Creating the FileWriter and the new File we are writing to
            FileStream outFile = new FileStream(@outputName, FileMode.CreateNew);
            BinaryWriter outWrite = new BinaryWriter(outFile);

            int bufferSize = 4096;
            byte[] buffer = new byte[bufferSize];
            int success;
            while ((success = inFile.Read(buffer, 0, bufferSize)) > 0)
            {
                //Console.WriteLine(success);
                outWrite.Write(EncryptData(buffer,key), 0, success);
            }

            inRead.Close();
            inFile.Close();
            outWrite.Close();
            outFile.Close();

        }
    }
}
