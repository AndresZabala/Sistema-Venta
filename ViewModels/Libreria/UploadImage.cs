using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ViewModels.Libreria
{
    public class UploadImage
    {
        private OpenFileDialog objFile = new OpenFileDialog();

        //Metodo que carga imagen seleccionada
        public void CargarImagen(PictureBox miPicture)
        {
            miPicture.WaitOnLoad = true;
            objFile.Filter = "Imagenes|*.png;*.jpg;*.gif;*.png;*.bmp"; //Se realizan los filtros correspondientes
            objFile.ShowDialog(); //Muestra la venta con los filtros indicados
            if (objFile.FileName != string.Empty)
            {
                miPicture.ImageLocation = objFile.FileName;
            }
            else
            {
                MessageBox.Show("No se ha seleccionado una imagen, intente de nuevo.","ADVERTENCIA",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }
        }

        //Metodo que guarda la imagen
        public Image ResizeImage(Image image, int ancho, int alto)
        {
            using (var imagenBitmap = new Bitmap(ancho, alto,PixelFormat.Format32bppArgb))
            {
                imagenBitmap.SetResolution(Convert.ToInt32(image.HorizontalResolution),
                    Convert.ToInt32(image.HorizontalResolution));

                using (var imagenGraphics = Graphics.FromImage(imagenBitmap))
                {
                    imagenGraphics.SmoothingMode = SmoothingMode.AntiAlias;
                    imagenGraphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    imagenGraphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    imagenGraphics.DrawImage(image, new Rectangle(0, 0, ancho, alto),
                        new Rectangle(0, 0, image.Width, image.Height), GraphicsUnit.Pixel);
                    MemoryStream imagenMemoryStream = new MemoryStream();
                    imagenBitmap.Save(imagenMemoryStream, ImageFormat.Png);
                    image = Image.FromStream(imagenMemoryStream);

                 }
                return image;
            }
        }

        //Metodo que convierte la Image en byteArray para poderla guardar en la BD
        public byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }

        //Se convierte imagen de tipo byteArray en Image
        public Image byteArrayToImage(byte[] byteArrayIn)
        {
            MemoryStream ms = new MemoryStream(byteArrayIn);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }
    }
}
