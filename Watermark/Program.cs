using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

namespace MyTexturePacker
{
    class Program
    {
        static void Process(string inPath, string inMine)
        {
            string[] files = Directory.GetFiles(inPath);
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
                    Image sourceImg = Image.FromFile(file);
                    Graphics sourceG = Graphics.FromImage(sourceImg);

                    if (sourceImg == null || sourceG == null)
                        continue;

                    // 绘制在右下角
                    p.X = sourceImg.Width;
                    p.Y = sourceImg.Height;
                    p.X -= imgMine.Width;
                    p.Y -= imgMine.Height;
                    sourceG.DrawImage(imgMine, p);

                    // 覆盖原文件
                    sourceImg.Save(file + tempExt);
                    sourceG.Dispose();
                    sourceImg.Dispose();
                    File.Delete(file);
                    File.Move(file + tempExt, file);

                    Console.WriteLine("处理：" + file);
                }
            }
        }
        static void Main(string[] args)
        {
            string inPath = @"E:\\liubo\\github\\Watermark\\临时美术\\origin";
            string inMine = @"E:\\liubo\\github\\Watermark\\临时美术\\Bad-Pig.png";

            inPath = Directory.GetCurrentDirectory() + "\\origin";
            inMine = Directory.GetCurrentDirectory() + "\\Bad-Pig.png";

            Process(inPath, inMine);

            Console.WriteLine("Press any key to continue . . .");
            Console.ReadKey(true);
        }

    }
}
