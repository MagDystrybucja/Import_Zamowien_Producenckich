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
        public Form1()
        {
            string[] args = Environment.GetCommandLineArgs();
            if (args.Length > 1)
            {
                if (args[1].ToUpper() == "AUTO")
                {
                    Formaty q = new Formaty(ref this.richEditControl1);
                    q.Asseco311_ReadFile(@"C:\Users\wojciech.bazydlo\Desktop\aa\f58bbc5c-f428-4a3d-a15c-4dd6baa11647.ZAM", 2);
                    MessageBox.Show("poszlo");
                }
                System.Environment.Exit(0);
            }
            else
            {
                InitializeComponent();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Formaty q = new Formaty(ref this.richEditControl1);
           // q.Importtest();
            /*
            List<string> aa= q.Synergizer_ExportFileLinesAsseco311(q.Asseco311_ReadFile(@"C:\Users\wojciech.bazydlo\Desktop\aa\f58bbc5c-f428-4a3d-a15c-4dd6baa11647.ZAM", 1), q.Asseco311_ReadFile(@"C:\Users\wojciech.bazydlo\Desktop\aa\f58bbc5c-f428-4a3d-a15c-4dd6baa11647.ZAM", 2));
            q.SaveFile(aa, @"C:\Users\wojciech.bazydlo\Desktop\aa\bb\f58bbc5c-f428-4a3d-a15c-4dd6baa11647.ZAM");
            List<string> aba = q.ListFolderFiles(@"C:\Users\wojciech.bazydlo\Desktop\aa\", "*.ZAM");*/
        }

        private void button2_Click(object sender, EventArgs e)
        {
        }

    }
}
