using ImageProcessor;
using ImageProcessor.Imaging;
using ImageProcessor.Imaging.Formats;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageProcess
{
    public enum ImageSize
    {
        SMALL = 1,
        MEDIUM = 2,
        LARGE = 3
    }
    public class ResizeImage
    {
        public void Resize(ImageSize imageSize, string file, string saveFolderPath)
        {
            byte[] photoBytes = File.ReadAllBytes(file);
            // Format is automatically detected though can be changed.
            ISupportedImageFormat format = new JpegFormat { Quality = 70 };
            Size size = GetSize(imageSize);
            using (MemoryStream inStream = new MemoryStream(photoBytes))
            {
                using (MemoryStream outStream = new MemoryStream())
                {
                    // Initialize the ImageFactory using the overload to preserve EXIF metadata.
                    using (ImageFactory imageFactory = new ImageFactory(preserveExifData: true))
                    {
                        // Load, resize, set the format and quality and save an image.
                        var resizeLayer = new ResizeLayer(size,ResizeMode.Max);
                        imageFactory.Load(inStream)
                                    .Resize(resizeLayer)
                                    .Format(format)
                                    .Save(outStream);
                    }
                    // Do something with the stream.
                    // Check if folder exists
                    if (!Directory.Exists(saveFolderPath))
                    {
                        Directory.CreateDirectory(saveFolderPath);// create when not exists
                    }
                    var filePath = GetFilePath(saveFolderPath, file, imageSize);
                    using (FileStream fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        fileStream.Write(outStream.GetBuffer(), 0, outStream.GetBuffer().Length);
                        fileStream.Close();
                    }
                }
            }

        }

        private Size GetSize(ImageSize imageSize)
        {
            var size = new Size();
            switch (imageSize)
            {
                case ImageSize.SMALL:
                    size = new Size(358, 164);
                    break;
                case ImageSize.MEDIUM:
                    size = new Size(705, 362);
                    break;
                case ImageSize.LARGE:
                    size = new Size(900, 462);
                    break;
            }
            return size;
        }

        private string GetFilePath(string filePath, string currentFullPath, ImageSize imageSize)
        {
            var fileNameSplits = currentFullPath.Split('\\');
            var fileNameWithExt = fileNameSplits[fileNameSplits.Length - 1];
            var fileName = fileNameWithExt.Split('.');
            var sizeAbbr = "";
            switch (imageSize)
            {
                case ImageSize.SMALL:
                    sizeAbbr = "sm";
                    break;
                case ImageSize.MEDIUM:
                    sizeAbbr = "md";
                    break;
                case ImageSize.LARGE:
                    sizeAbbr = "lg";
                    break;
            }
            string filePathAll = String.Format("{0}/{1}_{2}.{3}", filePath, fileName[0], sizeAbbr,fileName[1]);
            return filePathAll;
        }
    }
}
