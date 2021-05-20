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
                    InitializeComponent();
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

        private void Form1_Load(object sender, EventArgs e)
        {

            if (auto)
            {
                Formaty q = new Formaty(ref this.richEditControl1);
                System.Environment.Exit(0);
            }
        }
    }
}
