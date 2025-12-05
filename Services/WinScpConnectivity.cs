using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MasterApplication.Services
{
    public class WinScpConnectivity
    {
        public bool UploadSFTPFile(string host, string username, string password, string sourcefile, string destination, int port, DependancyInjection DI)
        {
            try
            {
                using (SftpClient client = new SftpClient(host, port, username, password))
                {
                    client.Connect();
                    client.ChangeDirectory(destination);
                    using (FileStream fs = new FileStream(sourcefile, FileMode.Open))
                    {
                        client.BufferSize = 4 * 1024;
                        client.UploadFile(fs, Path.GetFileName(sourcefile));
                    }
                }
            }
            catch (Exception ex)
            {
                DI.dBAccess.WriteErrorLog("Exception occured while uploading file on SFTP. Error ==> " + ex.ToString());
                return false;
                //throw ex;
            }
            return true;
        }
    }
}
