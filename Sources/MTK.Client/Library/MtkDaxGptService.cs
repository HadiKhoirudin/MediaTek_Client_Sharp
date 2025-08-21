using mtkclient.library.xflash;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace mtkclient.library
{
    internal class MtkDaxGptService
    {
        public static async Task<MtkGpt> ReadAsync(
            IMtkDevice device,
            MtkDaxFlashInfo flashInfo,
            CancellationToken cancellationToken
        )
        {
            using (MemoryStream gptStream = new MemoryStream())
            {
                await MtkDaxPartitionService.ReadAsync(
                    device,
                    flashInfo,
                    0L,
                    2 * flashInfo.PageSize,
                    gptStream,
                    cancellationToken
                );
                //Richlog("Parsing gpt header");
                MtkGpt gpt = MtkGptParserService.Parse(gptStream.ToArray(), flashInfo.PageSize);
                //Richlog("Reading gpt partitions");
                gptStream.SetLength(0L);
                await MtkDaxPartitionService.ReadAsync(
                    device,
                    flashInfo,
                    0L,
                    34 * flashInfo.PageSize,
                    gptStream,
                    cancellationToken
                );

                return MtkGptParserService.ParsePartitions(gpt, gptStream.ToArray());
            }
        }

        public static async Task<MtkGpt> ReadAsync(
            Stream inputStream,
            int pageSize,
            CancellationToken cancellationToken
        )
        {
            byte[] data = new byte[(34 * pageSize)];

            await inputStream.ReadAsync(data, 0, 2 * pageSize, cancellationToken);
            MtkGpt gpt = MtkGptParserService.Parse(data.Take(2 * pageSize).ToArray(), pageSize);

            await inputStream.ReadAsync(data, 2 * pageSize, 32 * pageSize, cancellationToken);
            return MtkGptParserService.ParsePartitions(gpt, data);
        }
    }
}
