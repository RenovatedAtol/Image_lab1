using System;
using System.Diagnostics;
using System.IO;



/* для подключения System.Drawing в своем проекте правой в проекте нажать правой кнопкой по Ссылкам -> Добавить ссылку
    отметить галочкой сборку System.Drawing    */
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.CompilerServices;
using System.Drawing.Imaging;

namespace IMGapp
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var img = new Bitmap("..\\..\\in.jpg"))    //открываем картинку     
            {     //блок using используется для корретного высвобождения памяти переменной, которая в нем создается
                  //для типа Bitmap это необходимо.
                  //вне блока using объект, в нем созданный, будет уже не доступен.
                  //Внутри этого блока using нельзя будет сохранить новое изображение в файл in.jpg,
                  //т.к. пока загруженный битмап висит в памяти файл открыт.

                Console.WriteLine("Открываю изображение " + Directory.GetParent("..\\..\\") + "\\in.jpg");
                
                
                var w = img.Width;
                var h = img.Height;
                
                using (var img_out = new Bitmap(w, h))   //создаем пустое изображение размером с исходное для сохранения результата
                {
                    var time1 = DateTime.Now;
                    Stopwatch timer = new Stopwatch();
                    timer.Start();

                    //попиксельно обрабатываем картинку 
                    for (int i = 0; i < h; ++i)
                    {
                        for (int j = 0; j < w; ++j)
                        {

                            //считывыем пиксель картинки и получаем его цвет
                            var pix = img.GetPixel(j, i);

                            //получаем цветовые компоненты цвета
                            int r = pix.R;
                            int g = pix.G;
                            int b = pix.B;

                            //lgbt flag
                            int h_av = h / 6;                            
                            switch (i / h_av)
                            {
                                case (0):
                                    //red
                                    r = (int)Clamp(r * 255, 0, 255);
                                    g = (int)Clamp(g * 0, 0, 255);
                                    b = (int)Clamp(b * 0, 0, 255);
                                    break;
                                case (1):
                                    //orange
                                    r = (int)Clamp(r * 128, 0, 255);
                                    g = (int)Clamp(g * 255, 0, 255);
                                    b = (int)Clamp(b * 0, 0, 255);
                                    break;
                                case (2):
                                    //yellow
                                    r = (int)Clamp(r * 255, 0, 255);
                                    g = (int)Clamp(g * 255, 0, 255);
                                    b = (int)Clamp(b * 0, 0, 255);
                                    break;
                                case (3):
                                    //green
                                    r = (int)Clamp(r * 0, 0, 255);
                                    g = (int)Clamp(g * 255, 0, 255);
                                    b = (int)Clamp(b * 0, 0, 255);
                                    break;
                                case (4):
                                    //blue
                                    r = (int)Clamp(r * 0, 0, 255);
                                    g = (int)Clamp(g * 0, 0, 255);
                                    b = (int)Clamp(b * 255, 0, 255);
                                    break;
                                case (5):
                                    //purple
                                    r = (int)Clamp(r * 128, 0, 255);
                                    g = (int)Clamp(g * 0, 0, 255);
                                    b = (int)Clamp(b * 255, 0, 255);
                                    break;
                            }
                               

                            //При вычислении пикселей используем функию Clamp (см. ниже Main) чтобы цвет не вылезал за границы [0 255]

                            //записываем пиксель в изображение
                            pix = Color.FromArgb(120, r, g, b);
                            img_out.SetPixel(j, i, pix);

                            //ц-ции GetPixel и SetPixel работают достаточно медленно, надо стримится к минимизации их использования
                        }
                    }

                    

                    timer.Stop();
                    
                    Console.WriteLine("Обработал изображение за " + timer.ElapsedMilliseconds + " мс.");

                    //сохраним нашу выходную картинку 
                    img_out.Save("..\\..\\lgbt.jpg");

                    System.Drawing.Image primaryImage = Image.FromFile(@"C:\Users\student\source\repos\Image_lab1\in.jpg");//or resource..

                    using (Graphics graphics = Graphics.FromImage(primaryImage))//get the underlying graphics object from the image.
                    {
                        System.Drawing.Image overlayImage = Image.FromFile(@"C:\Users\student\source\repos\Image_lab1\lgbt.jpg");
                           {
                            graphics.DrawImage(overlayImage, new Point(0, 0));//this will draw the overlay image over the base image at (0, 0) coordination.
                           }
                    }
                    System.Drawing.Image Control = primaryImage;
                    Control.Save("C:\\Users\\student\\source\\repos\\Image_lab1\\lgbtpic.png", ImageFormat.Png);


                    Console.ReadKey();

                } //using (var img_out = new Bitmap(w, h))     вот тут картинка img_out удаляется методом img_out.Dispose()     

            } //using (var img = new Bitmap("in.jpg"))   вот тут картинка img удаляется методом img.Dispose()     

        } //static void Main(string[] args)

        public static T Clamp<T>(T val, T min, T max) where T : IComparable<T>
        {
            if (val.CompareTo(min) < 0) return min;
            else if (val.CompareTo(max) > 0) return max;
            else return val;
        }
    }


}
