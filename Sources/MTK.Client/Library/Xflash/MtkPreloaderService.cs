using mtkclient.MTK.Client;
using System;
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace mtkclient.library.xflash
{
    internal class MtkPreloaderService
    {
        private static byte[] ConvertDwordsToByteArray(uint[] dwords)
        {
            using (MemoryStream memoryStream = new MemoryStream(dwords.Length * 4))
            {
                for (int i = 0; i < dwords.Length; i++)
                {
                    byte[] bytes = BitConverter.GetBytes(dwords[i]);
                    memoryStream.Write(bytes, 0, bytes.Length);
                }
                return memoryStream.ToArray();
            }
        }

        public static async Task<MtkPreloader> DumpAsync(
            IMtkDevice device,
            MtkChipConfig chipConfig,
            CancellationToken cancellationToken
        )
        {
            Console.WriteLine("Getting preloader index");
            byte[] data = ConvertDwordsToByteArray(
                await MtkReadWrite32Service.ReadResultAsync(
                    device,
                    2097152U,
                    16384,
                    little: true,
                    cancellationToken
                )
            );
            MtkPreloaderIndex index = MtkPreloaderParserService.ParseIndex(data);
            Console.WriteLine("Preloader index: {0}", index.ToString());
            Console.WriteLine("Delay for 150 ms");
            await Task.Delay(TimeSpan.FromMilliseconds(150.0));
            Console.WriteLine("Start dumping preloader data. Size: {0}", index.Length);
            using (MemoryStream preloaderStream = new MemoryStream())
            {
                int currentIndex = index.Index;
                int multiplier = 32;
                Main.SharedUI.label_totalsize.Invoke(
                    (Action)(
                        () => Main.SharedUI.label_totalsize.Text = utils.GetFileSize(index.Length)
                    )
                );

                Stopwatch Stopwatch = new Stopwatch();
                Stopwatch.Start();

                while (currentIndex - index.Index <= index.Length)
                {
                    uint num = (uint)(2097152 + currentIndex);

                    Main.SharedUI.label_writensize.Invoke(
                        (Action)(
                            () =>
                                Main.SharedUI.label_writensize.Text = utils.GetFileSize(
                                    currentIndex
                                )
                        )
                    );

                    TimeSpan elapsed = Stopwatch.Elapsed;
                    double speed = currentIndex / elapsed.TotalSeconds;
                    Main.SharedUI.label_transferrate.Invoke(
                        (Action)(
                            () =>
                                Main.SharedUI.label_transferrate.Text =
                                    utils.GetFileSize(Convert.ToInt64(speed)) + " /s"
                        )
                    );

                    Main.ProcessBar(currentIndex, index.Length);
                    data = ConvertDwordsToByteArray(
                        await MtkReadWrite32Service.ReadResultAsync(
                            device,
                            num,
                            multiplier * 4,
                            little: true,
                            cancellationToken
                        )
                    );
                    await preloaderStream.WriteAsync(data, 0, data.Length);
                    currentIndex += multiplier * 16;
                }
                Stopwatch.Stop();
                //Richlog(
                //    "Done dumping preloader data. Size: " + preloaderStream.Length,
                //    Color.Black,
                //    false,
                //    true
                //);
                preloaderStream.Seek(0L, SeekOrigin.Begin);
                return await LoadAsync(preloaderStream, chipConfig, cancellationToken);
            }
        }

        public static async Task<MtkPreloader> LoadAsync(
            Stream preloaderStream,
            MtkChipConfig chipConfig,
            CancellationToken cancellationToken
        )
        {
            byte[] preloaderBuff = new byte[(int)preloaderStream.Length];
            await preloaderStream.ReadAsync(
                preloaderBuff,
                0,
                preloaderBuff.Length,
                cancellationToken
            );
            string name = MtkPreloaderParserService.ParseName(preloaderBuff);
            MtkPreloaderEmi mtkPreloaderEmi = MtkPreloaderParserService.ParseEmi(
                preloaderBuff,
                chipConfig.UseXFlash
            );
            return new MtkPreloader(
                name,
                mtkPreloaderEmi.Version,
                mtkPreloaderEmi.Emi,
                preloaderBuff
            );
        }
    }
}
