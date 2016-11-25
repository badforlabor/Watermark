using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace MyTexturePacker
{
    class Program
    {
        static void Process(string inPath, string inMine)
        {
            string[] files = Directory.GetFiles(inPath, "*.*", SearchOption.AllDirectories);
            string tempExt = ".tmp";
            if (files.Length > 0)
            {
                Console.WriteLine("files.length=" + files.Length);

                // 创建一个512*512的图片
                Image imgMine = Image.FromFile(inMine);
                if (imgMine == null)
                    return;

                Point p = Point.Empty;
                foreach (var file in files)
                {
                    try
                    {
                        Image sourceImg = Image.FromFile(file);

                        int factor = 0;
                        int FormatWidth = 1080;
                        if (sourceImg.Width < FormatWidth || sourceImg.Height < FormatWidth)
                            factor = FormatWidth;
                        else if (sourceImg.Width < sourceImg.Height)
                            factor = sourceImg.Width;
                        else
                            factor = sourceImg.Height;

                        int width = sourceImg.Width * FormatWidth / factor;
                        int height = sourceImg.Height * FormatWidth / factor;

                        Image img = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                        Graphics sourceG = Graphics.FromImage(img);

                        if (sourceImg == null || sourceG == null)
                            continue;

                        // 缩放
                        sourceG.DrawImage(sourceImg, new Rectangle(0, 0, width, height),
                                    new Rectangle(0, 0, sourceImg.Width, sourceImg.Height), GraphicsUnit.Pixel);

                        // 右下角水印
                        p.X = img.Width;
                        p.Y = img.Height;
                        p.X -= imgMine.Width;
                        p.Y -= imgMine.Height;
                        sourceG.DrawImage(imgMine, p);

                        // 覆盖原文件
                        sourceImg.Dispose();
                        img.Save(file, ImageFormat.Jpeg);
                        sourceG.Dispose();
                        img.Dispose();
                        //                     sourceImg.Save(file + tempExt);
                        //                     sourceG.Dispose();
                        //                     File.Delete(file);
                        //                     File.Move(file + tempExt, file);

                        Console.WriteLine("处理：" + file);
                    }
                    catch (System.Exception ex)
                    {
                        Console.WriteLine("异常了：" + file);
                    }
                }
            }
        }
        static void Main(string[] args)
        {
            string inPath = @"D:\\liubo\\github\\Watermark\\临时美术\\origin";
            string inMine = @"D:\\liubo\\github\\Watermark\\临时美术\\Bad-Pig.png";

             inPath = Directory.GetCurrentDirectory() + "\\origin";
             inMine = Directory.GetCurrentDirectory() + "\\Bad-Pig.png";

            Process(inPath, inMine);

            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey(true);
        }

    }
}
