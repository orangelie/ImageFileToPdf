using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace PngToPdf
{
    public partial class Form1 : Form
    {
        List<string> lstFiles = new List<string>();

        public Form1() { InitializeComponent(); }
        private void Form1_Load(object sender, EventArgs e) { }

        public void ImagesToPdf(List<string> imagepaths)
        {
            iTextSharp.text.Rectangle pageSize = null;

            using (var srcImage = new Bitmap(imagepaths[0].ToString()))
            {
                pageSize = new iTextSharp.text.Rectangle(0, 0, srcImage.Width, srcImage.Height);
            }

            using (var ms = new MemoryStream())
            {
                var document = new iTextSharp.text.Document(pageSize, 0, 0, 0, 0);
                PdfWriter.GetInstance(document, ms).SetFullCompression();
                document.Open();

                foreach (string path in imagepaths)
                {
                    var image = iTextSharp.text.Image.GetInstance(path.ToString());
                    document.Add(image);
                }

                document.Close();

                File.WriteAllBytes("output.pdf", ms.ToArray());

                MessageBox.Show("PDF 변환 성공 !");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlgOpen = new OpenFileDialog())
            {
                dlgOpen.Filter = "JPG File|*.jpg|PNG File|*.png";
                dlgOpen.Title = "Select Audio File";
                dlgOpen.Multiselect = true; 

                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
                    lstFiles.Clear();
                    listBox1.Items.Clear();

                    for (int i = 0; i < dlgOpen.FileNames.Length; i++)
                    {
                        lstFiles.Add(dlgOpen.FileNames[i]);
                        listBox1.Items.Add(dlgOpen.FileNames[i]);
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ImagesToPdf(lstFiles);
        }
    }
}
