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
                #region GET NAME AND COUNT
                if (checkNameCount())
                {
                    var keyvalues = new Dictionary<string, int>();
                    foreach (var file in dialog.FileNames)
                    {
                        keyvalues = getNameAndCount(file, keyvalues);
                        process += 1;
                    }
                    var list = keyvalues.ToList();
                    StringBuilder sb = new StringBuilder();
                    foreach(var item in list)
                    {
                        sb.AppendLine(string.Format("{0}, {1}", item.Key, item.Value));
                    }
                    var result = sb.ToString();
                    return;
                }
                #endregion
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

        private bool checkNameCount()
        {
            if (this.radioNameCount.Checked)
            {
                return true;
            }
            return false;
        }

        private Dictionary<string,int> getNameAndCount(string currentFullPath, Dictionary<string, int> keyValues)
        {

            var fileNameSplits = currentFullPath.Split('\\');
            var fileNameWithExt = fileNameSplits[fileNameSplits.Length - 1];
            var fileName = fileNameWithExt.Split('.');
            var materialAndCount = fileName[0].Split('_');
            var key = materialAndCount[0];
            var value = Convert.ToInt32(materialAndCount[1]);
            if (keyValues.ContainsKey(key))
            {
                var nowValue = 0;
                keyValues.TryGetValue(key, out nowValue);
                if (value > nowValue)
                {
                    keyValues.Remove(key);
                    keyValues.Add(key, value);
                }
            }
            else
            {
                keyValues.Add(key, value);
            }
            return keyValues;
        }

    }
}
