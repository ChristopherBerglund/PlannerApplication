using SixLabors.ImageSharp;

namespace PlannerApplication.HelpClasses
{
    public class Cropper
    {
        public static string ResizeImage(Image img, int maxWidth, int maxHeight)
        {
            if(img.Width>maxWidth || img.Height > maxHeight)
            {
                double widthRatio = (double)img.Width / (double)maxWidth;
                double heightRatio = (double)img.Height / (double)maxHeight;
                double ratio = Math.Max(widthRatio, heightRatio);
                int newWidth = (int)(img.Width / ratio);
                int newHeight = (int)(img.Height / ratio);
                return newHeight.ToString() + "," + newWidth.ToString();
            }
            else
            {
                return img.Height.ToString() + "," + img.Width.ToString(); 
            }
        }
    }
}
