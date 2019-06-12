using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Dropbox.Api;
using Dropbox.Api.Files;

namespace SecurityApplication.Services
{
    public class DropBoxService
    {
        private static string _token = "ZiAbWay2XJAAAAAAAAAAEF9jljSxq_mKekhIfxB7_UFViYb8EK_8NsvvKJQd5M6O";
        private DropboxClient client = new DropboxClient(_token);

        public async Task<List<string>> ListRootFolder()
        {
            var list = await client.Files.ListFolderAsync(string.Empty);

            List<string> data = new List<string>();

            foreach (var item in list.Entries)
            {
                data.Add(item.Name);
            }
            return data;
        }

        public async Task Upload(string localPath, string remotePath)
        {
            const int ChunkSize = 4096 * 1024;
            using (var fileStream = File.Open(localPath, FileMode.Open))
            {
                if (fileStream.Length <= ChunkSize)
                {
                    await this.client.Files.UploadAsync(remotePath, body: fileStream);
                }
                else
                {
                    await this.ChunkUpload(remotePath, fileStream, (int)ChunkSize);
                }
            }
        }

        private async Task ChunkUpload(string path, FileStream stream, int chunkSize)
        {
            ulong numChunks = (ulong)Math.Ceiling((double)stream.Length / chunkSize);
            byte[] buffer = new byte[chunkSize];
            string sessionId = null;
            for (ulong idx = 0; idx < numChunks; idx++)
            {
                var byteRead = stream.Read(buffer, 0, chunkSize);

                using (var memStream = new MemoryStream(buffer, 0, byteRead))
                {
                    if (idx == 0)
                    {
                        var result = await this.client.Files.UploadSessionStartAsync(false, memStream);
                        sessionId = result.SessionId;
                    }
                    else
                    {
                        var cursor = new UploadSessionCursor(sessionId, (ulong)chunkSize * idx);

                        if (idx == numChunks - 1)
                        {
                            FileMetadata fileMetadata = await this.client.Files.UploadSessionFinishAsync(cursor, new CommitInfo(path), memStream);
                            Console.WriteLine(fileMetadata.PathDisplay);
                        }
                        else
                        {
                            await this.client.Files.UploadSessionAppendV2Async(cursor, false, memStream);
                        }
                    }
                }
            }
        }



        //public async Task Upload(string uri)
        //{
        //    using (var client = new DropboxClient(_token))
        //    using (var stream = new MemoryStream())
        //    {
        //        var updated = await client.Files.UploadAsync(
        //            uri,
        //            WriteMode.Overwrite.Instance,
        //            body: stream);
        //    }

        //}

    }
}
