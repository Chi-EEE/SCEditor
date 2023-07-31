using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Windows.Forms;
using Microsoft.Win32;
using System.Windows.Markup;
using SCEditor.Helpers;
using SCEditor.ScOld;
using System.IO;

namespace SCEditor
{
    internal static class Program
    {
        [SupportedOSPlatform("windows")]
        [DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = CharSet.Auto,
            CallingConvention = CallingConvention.StdCall)]
        public static extern int AllocConsole();
        private static ImageCodecInfo GetEncoderInfo(String mimeType)
        {
            int j;
            ImageCodecInfo[] encoders;
            encoders = ImageCodecInfo.GetImageEncoders();
            for (j = 0; j < encoders.Length; ++j)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            return null;
        }

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            #region Debug
#if DEBUG
            AllocConsole();
            Console.SetOut(new Prefixed());
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("SC Editor | Development Edition");
#endif
            #endregion
            String path_1 = args[0];
            String path_2 = args[1];
            Console.WriteLine(path_1);
            Console.WriteLine(path_2);
            ScFile scFile = new ScFile(path_1, path_2);
            scFile.Load();
            ImageCodecInfo myImageCodecInfo = GetEncoderInfo("image/png");
            Encoder myEncoder = Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, 100L);
            myEncoderParameters.Param[0] = myEncoderParameter;
            DirectoryInfo di = new DirectoryInfo(path_1);
            foreach (Export export in scFile.GetExports())
            {
                MovieClip movieClip = (MovieClip)export.GetDataObject();
                for (int frameIndex = 0; frameIndex < movieClip.Frames.Count; frameIndex++)
                {
                    movieClip.initPointFList(null);
                    movieClip.renderAnimation(new RenderingOptions(), frameIndex).Save(Path.Combine(di.Parent.FullName, $"{export.GetName()}_frame_{(frameIndex + 1).ToString("D6")}.png"), myImageCodecInfo, myEncoderParameters);
                }
                movieClip.destroyPointFList();
            }
            #region Debug
#if DEBUG

            Console.WriteLine("Debugging done");
            Console.ReadLine();

#endif
            #endregion
        }
    }
}
