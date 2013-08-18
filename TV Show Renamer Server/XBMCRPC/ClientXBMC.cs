using System;
using System.Linq;
using System.Net;
using System.IO;
using System.Threading.Tasks;


namespace XBMCRPC
{
    /// <summary>
    /// This partial class contains some additional XBMC specific methods
    /// </summary>
    public partial class Client
    {
        //this is required for the current workaround for multiple inheritances in json schema
        private const string MultipleInheritanceKeyAll = "lyrics";
        private const string MultipleInheritanceKeyBase = "lyrics";
        private const string MultipleInheritanceKeyFile = "lyrics";

		public Client(string host, int port=8080, string userName="xbmc", string password=""):this(new ConnectionSettings(host,port,userName,password))
        {
        }

        /// <summary>
        /// Gets a stream of the image thumbnail.
        /// </summary>
        /// <param name="thumbnailUri">The thumbnail URI.</param>
        /// <returns></returns>
        public  Stream GetImageStream(string thumbnailUri)
        {
            string downloadPath; 
            if (thumbnailUri.StartsWith("image:"))
            {
                var thumbnailEncoded = Uri.EscapeDataString(thumbnailUri);
                downloadPath = "image/" + thumbnailEncoded;
            }
            else
            {
                var download =  Files.PrepareDownload(thumbnailUri);
                downloadPath = download.details["path"].ToString();
            }
                        
            var request =  WebRequest.Create(new Uri( _settings.BaseAddress + downloadPath));
            request.Credentials = new NetworkCredential(_settings.UserName, _settings.Password);

            var response =  request.GetResponse();
            return response.GetResponseStream();
        }

        ///// <summary>
        ///// Saves a downloaded image to a file
        ///// </summary>
        ///// <param name="thumbnailUri">The thumbnail URI</param>
        ///// <param name="downloadFile">Path of downloaded file</param>
        ///// <returns></returns>
        //public  Task DownloadImage(string thumbnailUri, string downloadFile)
        //{
        //    var httpStream =  GetImageStream(thumbnailUri);
        //    var fileStream = File.OpenWrite(downloadFile);
        //     httpStream.CopyTo(fileStream);
        //    fileStream.Close();
        //    httpStream.Close();
        //}

        /// <summary>
        /// Returns all values of an enum in an array
        /// </summary>
        /// <typeparam name="TEnum">Type name of the enum</typeparam>
        /// <returns></returns>
        public static TEnum[] AllValues<TEnum>()
        {
            return Enum.GetValues(typeof(TEnum)).Cast<TEnum>().ToArray();
        }
    }
}