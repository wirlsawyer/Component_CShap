using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Component
{
    public class NSINI
    {
        #region WIN32API
        [DllImport("kernel32.dll", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode, ExactSpelling = true, SetLastError = true)]
        private static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault,
           [MarshalAs(UnmanagedType.LPArray, SizeParamIndex = 4)] char[] lpReturnedString, int nSize, string lpFileName);


        [DllImport("kernel32", EntryPoint = "GetPrivateProfileStringW", CharSet = CharSet.Unicode, ExactSpelling = true)]
        static extern int GetPrivateProfileString(string Section, string Key, string Value, StringBuilder Result, int Size, string FileName);


        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(string Section, int Key, string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result, int Size, string FileName);

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section,
            string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileInt(string section, string key, int def, string filePath);

        
        [DllImport("kernel32")]
        static extern int GetPrivateProfileString(int Section, string Key,
               string Value, [MarshalAs(UnmanagedType.LPArray)] byte[] Result,
               int Size, string FileName);
        #endregion

        #region Get
        static public String GetPrivateProfile(String fileName, String sectionName, String keyName, String lpDefault)
        {
            StringBuilder temp = new StringBuilder(1024);
            GetPrivateProfileString(sectionName, keyName, lpDefault, temp, 1024, fileName);
            return temp.ToString();

            char[] ret = new char[256];


            while (true)
            {
                int length = GetPrivateProfileString(sectionName, keyName, null, ret, ret.Length, fileName);
                if (length == 0)
                {
                    return lpDefault;
                }


                // This function behaves differently if both sectionName and keyName are null
                if (sectionName != null && keyName != null)
                {
                    if (length == ret.Length - 1)
                    {
                        // Double the buffer size and call again
                        ret = new char[ret.Length * 2];
                    }
                    else
                    {
                        // Return simple string
                        return new string(ret, 0, length);
                    }
                }
                else
                {
                    if (length == ret.Length - 2)
                    {
                        // Double the buffer size and call again
                        ret = new char[ret.Length * 2];
                    }
                    else
                    {
                        // Return multistring
                        return new string(ret, 0, length - 1);
                    }
                }
            }
        }

        static public int GetPrivateProfile(String fileName, String sectionName, String keyName, int lpDefault)
        {
            return GetPrivateProfileInt(sectionName, keyName, lpDefault, fileName);
        }

        static public String[] GetSectionNames(String fileName)
        {
            //    Sets the maxsize buffer to 500, if the more
            //    is required then doubles the size each time.
            for (int maxsize = 500; true; maxsize *= 2)
            {
                //    Obtains the information in bytes and stores
                //    them in the maxsize buffer (Bytes array)
                byte[] bytes = new byte[maxsize];
                int size = GetPrivateProfileString(0, "", "", bytes, maxsize, fileName);

                // Check the information obtained is not bigger
                // than the allocated maxsize buffer - 2 bytes.
                // if it is, then skip over the next section
                // so that the maxsize buffer can be doubled.
                if (size < maxsize - 2)
                {
                    // Converts the bytes value into an ASCII char. This is one long string.
                    string Selected = Encoding.ASCII.GetString(bytes, 0,
                                               size - (size > 0 ? 1 : 0));
                    // Splits the Long string into an array based on the "\0"
                    // or null (Newline) value and returns the value(s) in an array
                    return Selected.Split(new char[] { '\0' });
                }
            }
        }

        static public String[] GetEntryNames(String fileName, string section)
        {
            //    Sets the maxsize buffer to 500, if the more
            //    is required then doubles the size each time. 
            for (int maxsize = 500; true; maxsize *= 2)
            {
                //    Obtains the EntryKey information in bytes
                //    and stores them in the maxsize buffer (Bytes array).
                //    Note that the SectionHeader value has been passed.
                byte[] bytes = new byte[maxsize];
                int size = GetPrivateProfileString(section, 0, "", bytes, maxsize, fileName);

                // Check the information obtained is not bigger
                // than the allocated maxsize buffer - 2 bytes.
                // if it is, then skip over the next section
                // so that the maxsize buffer can be doubled.
                if (size < maxsize - 2)
                {
                    // Converts the bytes value into an ASCII char.
                    // This is one long string.
                    string entries = Encoding.ASCII.GetString(bytes, 0,
                                              size - (size > 0 ? 1 : 0));
                    // Splits the Long string into an array based on the "\0"
                    // or null (Newline) value and returns the value(s) in an array
                    return entries.Split(new char[] { '\0' });
                }
            }
        }

        // The Function called to obtain the EntryKey Value from
        // the given SectionHeader and EntryKey string passed, then returned
        public object GetEntryValue(String fileName, String section, String entry)
        {
            //    Sets the maxsize buffer to 250, if the more
            //    is required then doubles the size each time. 
            for (int maxsize = 250; true; maxsize *= 2)
            {
                //    Obtains the EntryValue information and uses the StringBuilder
                //    Function to and stores them in the maxsize buffers (result).
                //    Note that the SectionHeader and EntryKey values has been passed.
                StringBuilder result = new StringBuilder(maxsize);
                int size = GetPrivateProfileString(section, entry, "",
                                                   result, maxsize, fileName);
                if (size < maxsize - 1)
                {
                    // Returns the value gathered from the EntryKey
                    return result.ToString();
                }
            }
        }

        #endregion

        #region Write
        static public void WritePrivateProfile(String fileName, String sectionName, String keyName, int iVal)
        {
            WritePrivateProfileString(sectionName, keyName, iVal.ToString(), fileName);
        }
        static public void WritePrivateProfile(String fileName, String sectionName, String keyName, String strVal)
        {
            WritePrivateProfileString(sectionName, keyName, strVal, fileName);
        }
        #endregion

        #region Delete
        static public void DelKey(String fileName, String sectionName, String keyName)
        {
            WritePrivateProfileString(sectionName, keyName, null, fileName);
        }
        static public void DelSection(String fileName, String sectionName)
        {
            WritePrivateProfileString(sectionName, null, null, fileName);
        }
        #endregion

    }
}

