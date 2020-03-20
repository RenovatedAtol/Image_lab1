using System;
using System.IO;



/* для подключения System.Drawing в своем проекте правой в проекте нажать правой кнопкой по Ссылкам -> Добавить ссылку
    отметить галочкой сборку System.Drawing    */
using System.Drawing;
using System.Drawing.Drawing2D;


namespace IMGapp
{

    class Program
    {
        static void Main(string[] args)
        {
            try
            {

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {

            }

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

                            //Увеличим квет каждого пикселя на 1.4
                            //При вычислении пикселей используем функию Clamp (см. ниже Main) чтобы цвет не вылезал за границы [0 255]
                            r = (int)Clamp(r * 1.4, 0, 255);
                            g = (int)Clamp(g * 1.4, 0, 255);
                            b = (int)Clamp(b * 1.4, 0, 255);


                            //записываем пиксель в изображение
                            pix = Color.FromArgb(r, g, b);
                            img_out.SetPixel(j, i, pix);

                            //ц-ции GetPixel и SetPixel работают достаточно медленно, надо стримится к минимизации их использования
                        }
                    }

                    //нарисуем что нибудь на картинке
                    using (var g = Graphics.FromImage(img_out)) //через Using создадим объекет Graphics из нашей выходной картинке
                    {              //Graphics как раз содержит методы для рисования линий, текста и прочих геомиетричсеких примитивов

                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = SmoothingMode.HighQuality;

                        var p = Pens.Red.Clone() as Pen;  //красная ручка
                        p.Width = 5;        //Так как кисть стандартная, для изменения ее свойств создадим ее копию ф-цией Clone

                        
                        g.FillRectangle(Brushes.White, 10, 10, 340, 50); //белый прямоугольник

                        var f = new Font("Times New Roman", 20, FontStyle.Bold); //шрифт
                        g.DrawString("Выходное изображение:", f, Brushes.Black, 10, 10);

                        g.DrawLine(p, 10, 10, 350, 10); //красная линия     

                        //В завершении, нарисуем зеленую синусоиду на картинке =)m
                        var green_pen = Pens.Green.Clone() as Pen;
                        green_pen.Width = 3;
                        for (int i = 1; i < w; ++i)
                            g.DrawLine(green_pen, 
                                (i - 1), 
                                h/2 + (int)(50 * Math.Sin((i - 1) / 50.0)), 
                                i, 
                                h/2 + (int)(50 * Math.Sin(i / 50.0)));

                        p.Dispose();
                        green_pen.Dispose();
                    }     //вот тут графикс g удаляется методом g.Dispose()     

                    var time2 = DateTime.Now;
                    Console.WriteLine("Обработал изображение за " + Math.Round((time2-time1).TotalMilliseconds) + " мс.");
                   
                    //сохраним нашу выходную картинку 
                    img_out.Save("..\\..\\out.jpg");
                    
                    
                    Console.WriteLine("Выходное изображение было сохренено по пути " + Directory.GetParent("..\\..\\") + "\\out.jpg");
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
