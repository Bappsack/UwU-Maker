using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;

namespace UwU_Maker
{

    class SearchImage
    {

        [DllImport("user32.dll", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GetDC(IntPtr hWnd);

        [DllImport("user32.dll", ExactSpelling = true)]
        public static extern IntPtr ReleaseDC(IntPtr hWnd, IntPtr hDC);

        [DllImport("gdi32.dll", ExactSpelling = true)]
        public static extern IntPtr BitBlt(IntPtr hDestDC, int x, int y, int nWidth, int nHeight, IntPtr hSrcDC, int xSrc, int ySrc, int dwRop);


        public struct RECT
        {
            public int Left;
            public int Top;
            public int Right;
            public int Bottom;
        }

        public static Bitmap CaptureWindow(IntPtr hwnd, int Width, int Height)
        {
            Bitmap screenBmp = new Bitmap(Width, Height);
            Graphics g = Graphics.FromImage(screenBmp);

            IntPtr dc1 = GetDC(hwnd);
            IntPtr dc2 = g.GetHdc();

            //Main drawing, copies the screen to the bitmap
            //last number is the copy constant
            BitBlt(dc2, 0, 0, Width, Height, dc1, 0, 0, 13369376);

            //Clean up
            ReleaseDC(hwnd, dc1);
            g.ReleaseHdc(dc2);
            g.Dispose();

            return screenBmp;
        }

        public static Point Find(Bitmap haystack, Bitmap needle, out Point point)
        {
            point = Point.Empty;
            int Now = DateTime.Now.Millisecond;
            if (null == haystack || null == needle)
            {
                return Point.Empty;
            }
            if (haystack.Width < needle.Width || haystack.Height < needle.Height)
            {
                return Point.Empty;
            }

            var haystackArray = GetPixelArray(haystack);
            var needleArray = GetPixelArray(needle);

            foreach (var firstLineMatchPoint in FindMatch(haystackArray.Take(haystack.Height - needle.Height), needleArray[0]))
            {
                if (IsNeedlePresentAtLocation(haystackArray, needleArray, firstLineMatchPoint, 1))
                {
                    point = firstLineMatchPoint;
                    return firstLineMatchPoint;
                }
            }
            return Point.Empty;
        }

        public static Color GetPixel(Bitmap bitmap, int X, int Y)
        {
            LockBitmap bit = new LockBitmap(bitmap);
            bit.LockBits();
            Color color = bit.GetPixel(X, Y);
            bit.UnlockBits();
            return color;
        }

        public static Point PixelSearch(Bitmap image, Color color)
        {
            List<Point> result = new List<Point>();
            for (int x = 0; x < image.Width; x++)
            {
                for (int y = 0; y < image.Height; y++)
                {
                    if (color.Equals(image.GetPixel(x, y)))
                    {
                        return new Point(x, y);
                    }
                }
            }

            return Point.Empty;
        }

        public static Bitmap GetBitmapArea(Bitmap bitmap, int fX, int fY, int sX, int sY)
        {
            /*
            Draw a Rectangle from two X,Y Coordinations 
            2th pos - 1th pos == Size of Rectangle
            */

            Rectangle r = new Rectangle(fX, fY, (sX - fX), (sY - fY));
            Bitmap nb = new Bitmap(r.Width, r.Height);
            Graphics g = Graphics.FromImage(nb);
            g.DrawImage(bitmap, -r.X, -r.Y);
            return nb;
        }

        private static int[][] GetPixelArray(Bitmap bitmap)
        {
            var result = new int[bitmap.Height][];
            var bitmapData = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);

            for (int y = 0; y < bitmap.Height; ++y)
            {
                result[y] = new int[bitmap.Width];
                Marshal.Copy(bitmapData.Scan0 + y * bitmapData.Stride, result[y], 0, result[y].Length);
            }

            bitmap.UnlockBits(bitmapData);

            return result;
        }

        private static IEnumerable<Point> FindMatch(IEnumerable<int[]> haystackLines, int[] needleLine)
        {
            var y = 0;
            foreach (var haystackLine in haystackLines)
            {
                for (int x = 0, n = haystackLine.Length - needleLine.Length; x < n; ++x)
                {
                    if (ContainSameElements(haystackLine, x, needleLine, 0, needleLine.Length))
                    {
                        yield return new Point(x, y);
                    }
                }
                y += 1;
            }
        }

        private static bool ContainSameElements(int[] first, int firstStart, int[] second, int secondStart, int length)
        {
            for (int i = 0; i < length; ++i)
            {
                if (first[i + firstStart] != second[i + secondStart])
                {
                    return false;
                }
            }
            return true;
        }

        private static bool IsNeedlePresentAtLocation(int[][] haystack, int[][] needle, Point point, int alreadyVerified)
        {
            //we already know that "alreadyVerified" lines already match, so skip them
            for (int y = alreadyVerified; y < needle.Length; ++y)
            {
                if (!ContainSameElements(haystack[y + point.Y], point.X, needle[y], 0, needle[y].Length))
                {
                    return false;
                }
            }
            return true;
        }
    }
}