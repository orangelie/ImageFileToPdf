using com.itextpdf.text.pdf;
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
        int idx = -1;
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

                File.WriteAllBytes("Output.pdf", ms.ToArray());

                MessageBox.Show("PDF 변환 성공 !");
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog dlgOpen = new OpenFileDialog())
            {
                dlgOpen.Filter = "JPG File|*.jpg|PNG File|*.png|All Files|*.*";
                dlgOpen.Title = "Select File";
                dlgOpen.Multiselect = true;
                dlgOpen.RestoreDirectory = true;

                if (dlgOpen.ShowDialog() == DialogResult.OK)
                {
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

        private void button3_Click(object sender, EventArgs e)
        {
            lstFiles.Clear();
            listBox1.Items.Clear();
        }

        private void listBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (listBox1.SelectedIndex == -1)
                return;

            idx = listBox1.SelectedIndex;
        }

        private void Swap(int i1, int i2)
        {
            var temp = listBox1.Items[i1];
            listBox1.Items[i1] = listBox1.Items[i2];
            listBox1.Items[i2] = temp;

            var t = lstFiles[i1];
            lstFiles[i1] = lstFiles[i2];
            lstFiles[i2] = t;
        }

        private void listBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Down)
            {
                if (lstFiles.Count - 1 <= idx)
                    return;

                Swap(idx, idx + 1);
                idx++;
            }
            else if (e.KeyCode == Keys.Up)
            {
                if (0 >= idx)
                    return;

                Swap(idx, idx - 1);
                idx--;
            }
            else if (e.KeyCode == Keys.Return)
            {
                if (listBox1.SelectedIndex == -1)
                    return;

                using (Form form = new Form())
                {
                    PictureBox pictureBox = new PictureBox();
                    pictureBox.Dock = DockStyle.Fill;
                    pictureBox.Image = Image.FromFile(lstFiles[listBox1.SelectedIndex]);

                    form.Size = pictureBox.Image.Size;

                    form.Controls.Add(pictureBox);
                    form.ShowDialog();
                }
            }
        }

        private void listBox1_DoubleClick(object sender, EventArgs e)
        {
            if (idx == -1)
                return;

            using (Form form = new Form())
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Dock = DockStyle.Fill;
                pictureBox.Image = Image.FromFile(lstFiles[listBox1.SelectedIndex]);

                form.Size = pictureBox.Image.Size;

                form.Controls.Add(pictureBox);
                form.ShowDialog();
            }
        }

        private void Form1_DoubleClick(object sender, EventArgs e)
        {
            using (Form form = new Form())
            {
                form.Text = "사용법";
                form.Size = new Size(400, 100);

                Label lbl = new Label();
                lbl.Dock = DockStyle.Fill;
                lbl.Text =
                    "[ Ctrl ] > 사용방법 창\n" +
                    "[ ↓ ] > 아래파일과 순서 바꾸기\n" +
                    "[ ↑ ] > 위파일과 순서 바꾸기\n" +
                    "[ Enter ] 또는 [ 마우스 더블클릭 ] > 현재 선택된 파일 사진 띄우기\n" +
                    "[ 마우스 클릭 ] > 파일 선택";

                form.Controls.Add(lbl);
                form.ShowDialog();
            }
        }
    }
}
