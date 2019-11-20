using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImageProcess
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// open files
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (this.savePathTxt.Text.Length == 0)
            {
                MessageBox.Show("Please input save path first!");
                return;
            }
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = true;
            dialog.Title = "Please select files";
            dialog.Filter = "All files(*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                var process = 0;
                var len = dialog.FileNames.Length;
                ResizeImage imageHelper = new ResizeImage();
                MessageBox.Show(String.Format("{0} files selected", len));
                foreach (var file in dialog.FileNames)
                {
                    imageHelper.Resize(getSelectedSize(), file, this.savePathTxt.Text);
                    process += 1;
                    this.progressBar.Value = len == 0 ? 100 : 100 * process / len;
                }
            }
        }

        private ImageSize getSelectedSize()
        {
            ImageSize size = ImageSize.SMALL;
            if (this.radioSmall.Checked) { size = ImageSize.SMALL; }
            if (this.radioMedium.Checked) { size = ImageSize.MEDIUM; }
            if (this.radioLarge.Checked) { size = ImageSize.LARGE; }
            return size;
        }

    }
}
