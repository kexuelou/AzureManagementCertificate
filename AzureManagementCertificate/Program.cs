using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace AzureManagementCertificate
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("AzureManagementCertificate -pf YourPublishSettingsFile");
                Console.WriteLine(@"To download your Publish Setting File, visit https://windows.azure.com/download/publishprofile.aspx");
                return;
            }
            string strCert = "";
            bool valid = false;
            using (StreamReader sr = new StreamReader(args[0])) 
            {
                while (sr.Peek() >= 0) 
                {
                    strCert = sr.ReadLine();
                    if (strCert.TrimStart().StartsWith("ManagementCertificate"))
                    {
                        valid = true;
                        break;
                    }

                }
            }

            if (!valid)
                return;

            strCert = strCert.Substring(strCert.IndexOf('"')+1);
            strCert = strCert.Substring(0, strCert.IndexOf('"'));

            X509Certificate2 azureCert= new X509Certificate2(Convert.FromBase64String(strCert), "", X509KeyStorageFlags.Exportable);
            var store = new X509Store(StoreName.My, StoreLocation.CurrentUser);
            store.Open(OpenFlags.ReadWrite);
            if (!store.Certificates.Contains(azureCert))
            {
                store.Add(azureCert);
            }

        } 

    }
}
