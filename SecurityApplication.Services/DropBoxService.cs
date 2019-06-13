using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace SecurityApplication.Services
{
    public class DropBoxService
    {
        private static readonly string _token = "ZiAbWay2XJAAAAAAAAAAEF9jljSxq_mKekhIfxB7_UFViYb8EK_8NsvvKJQd5M6O";
        private DropboxClient client = new DropboxClient(_token);

        public async Task<List<Models.File>> ListRootFolder()
        {
            var list = await client.Files.ListFolderAsync(string.Empty);

            List<Models.File> data = new List<Models.File>();

            foreach (var item in list.Entries)
            {
                Models.File file = new Models.File { Name = item.Name };
                data.Add(file);
            }
            return data;
        }

        public async Task Upload(string localPath)
        {
            string file = localPath;
            string folder = "";
            string filename = localPath.Remove(0, localPath.LastIndexOf('\\') + 1);

            using (var dbx = new DropboxClient(_token))
            {
                using (var mem = new MemoryStream(File.ReadAllBytes(file)))
                {
                    var updated = dbx.Files.UploadAsync(folder + "/" + filename, WriteMode.Overwrite.Instance, body: mem);
                    updated.Wait();
                    var tx = dbx.Sharing.CreateSharedLinkWithSettingsAsync(folder + "/" + filename);
                    tx.Wait();
                }
            }
        }
    }


}

