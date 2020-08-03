using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Import_Zamowien_Producenckich.SQL;


namespace Import_Zamowien_Producenckich
{
    public partial class Form1 : Form
    {
        public bool auto = false;
        public Form1()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                if (args[1].ToUpper() == "AUTO")
                {
                    auto = true;
                }
            }
            else
            {
                InitializeComponent();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Formaty q = new Formaty(ref this.richEditControl1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

        private void Form1_Load(object sender, EventArgs e)
        {

            MessageBox.Show(auto.ToString());
            if (auto)
            {
                MessageBox.Show("QWE3");
                Formaty q = new Formaty(ref this.richEditControl1);
                MessageBox.Show("QWE");
                this.Close();
                MessageBox.Show("QWE2");
            }
        }
    }
}
