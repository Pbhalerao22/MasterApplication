using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MasterApplication.Services
{
    public class CreateTextFile
    {
        public string GenerateTXT(string FilePath, DataSet dsData, string FileName)
        {
            string genFilePath = "";
            try
            {
                FilePath = Path.Combine(FilePath, FileName);
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                StreamWriter sw = new StreamWriter(FilePath, false, Encoding.GetEncoding(1250));
                sw.NewLine = "\n";

                for (int i = 0; i < dsData.Tables[0].Rows.Count; i++)
                {
                    string newline = "";
                    newline = dsData.Tables[0].Rows[i]["OUTPUT"].ToString();
                    sw.WriteLine(newline);
                }
                sw.Close();
                genFilePath = FilePath;
            }
            catch (Exception ex)
            {
                genFilePath = "";
            }
            return genFilePath;
        }
    }
}
