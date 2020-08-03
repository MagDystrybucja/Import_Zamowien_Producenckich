using DevExpress.Office.Utils;
using DevExpress.XtraRichEdit.API.Native;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Import_Zamowien_Producenckich.SQL;
using DevExpress.Skins;
using DevExpress.LookAndFeel;
using DevExpress.UserSkins;
using DevExpress.XtraBars.Helpers;
using DevExpress.XtraBars;
using DevExpress.XtraBars.Ribbon;
using System.Net.Mail;
using System.Net;

namespace Import_Zamowien_Producenckich
{
    internal class Formaty
    {
        private int IdProducenta;
        private Dictionary<string,string> listaSkrotProd;
        private List<MAG_EBIZAM_Konfig> konfiguracja = new List<MAG_EBIZAM_Konfig>();
        private DevExpress.XtraRichEdit.RichEditControl rec1 = new DevExpress.XtraRichEdit.RichEditControl();
        public Formaty(ref DevExpress.XtraRichEdit.RichEditControl rec)
        {
            rec1 = rec;
            PobierzKonfiguracje();
            PrzetworzPliki();
        }
        public Formaty(int _IdProducenta)
        {
            IdProducenta = _IdProducenta;
        }
        public void PrzetworzPliki()
        {
            PobierzKonfiguracje();
            foreach (MAG_EBIZAM_Konfig producent in konfiguracja)
            {
                switch (producent.EZam_PlikZrdFormat)
                {
                    case "ASSECO311":
                        ImportASSECO311(producent.EZam_FoldZrd, producent.EZam_PlikZrdRozsz, producent.EZam_FoldArch,producent.EZam_ProdAkr,producent.EZam_ProdNazwa);
                        break;
                    default:
                        break;
                }
            }
            WeryfikacjaZamowien();
            SendEmail();
            GenerowaniePlikowMobius();
        }
        public void SendEmail()
        {
            DataTable zamok = Zamowienia_DoWyslaniaOK().Result;
            foreach (DataRow r in zamok.Rows)
            {
                #region Pobieranie danych i tworzenie ciała maila.
                rec1.CreateNewDocument();
                rec1.LoadDocument("Zamowienie_OK.docx");
                string Status_Zam="";
                string Producent;
                string PH_Producent;
                string Numer_Zam;
                string FileDate;
                string ZamDate;
                string Uwagi;
                string Rodzaj_zam;
                string XL_Akronim;
                string Prod_Akronim;
                string Prod_Adres;
                if (bool.TryParse(r["Zpn_Alert"].ToString(), out bool al))
                {
                    if (al)
                    { Status_Zam = "Błędy mapowania towarów. Obsługa CallCenter";  }
                    else
                    { Status_Zam = "OK. Wysłane do Mobiusa."; }
                }
                else { Status_Zam = "OK. Wysłane do Mobiusa."; }
                if (r["Zpn_Producent"].ToString().Length > 1) { Producent = r["Zpn_Producent"].ToString(); } else { Producent = "BŁĄD"; }
                if (r["Zpn_NAZWISKO"].ToString().Length > 1) { PH_Producent = r["Zpn_NAZWISKO"].ToString() + " " + r["Zpn_IMIE"].ToString(); } else { PH_Producent = "BŁĄD"; }
                if (r["Zpn_NR_ZAM"].ToString().Length > 1) { Numer_Zam = r["Zpn_NR_ZAM"].ToString(); } else { Numer_Zam = "BŁĄD"; }
                if (DateTime.TryParse(r["Zpn_DataPliku"].ToString(), out DateTime Data_Otrz)) { FileDate = Data_Otrz.ToShortDateString() + " " + Data_Otrz.ToShortTimeString(); } else { FileDate = "BŁĄD"; }
                if (DateTime.TryParse(r["Zpn_DATA_ZAM"].ToString(), out DateTime Data_Zam)) { ZamDate = Data_Zam.ToShortDateString() + " " + Data_Zam.ToShortTimeString(); } else { ZamDate = "BŁĄD"; }
                if (r["Zpn_UWAGI"].ToString().Length > 1) { Uwagi = r["Zpn_UWAGI"].ToString(); } else { Uwagi = "BŁĄD"; }
                if (r["Zpn_TYP_ZAM"].ToString().Length > 1) { if (r["Zpn_TYP_ZAM"].ToString() == "ZG") { Rodzaj_zam = "Gratisowe"; } else { Rodzaj_zam = "Standard"; } } else { Rodzaj_zam = "BŁĄD"; }
                if (r["Knt_Akronim"].ToString().Length > 1) { XL_Akronim = r["Knt_Akronim"].ToString(); } else { XL_Akronim = "BŁĄD"; }
                if (r["Zpn_NAZWA"].ToString().Length > 1) { Prod_Akronim = r["Zpn_NAZWA"].ToString(); } else { Prod_Akronim = "BŁĄD"; }
                if ((r["Zpn_MIASTO"].ToString()+ r["Zpn_ULICA"].ToString()).Length > 1) { Prod_Adres = r["Zpn_MIASTO"].ToString() +", "+ r["Zpn_ULICA"].ToString(); } else { Prod_Adres = "BŁĄD"; }
                if (Int32.TryParse(r["Zpn_Id"].ToString(), out int Zpn_id)) {  } else { Zpn_id = 0; }

                rec1.Document.ReplaceAll("<@Status_Zam>",Status_Zam , SearchOptions.None);
                rec1.Document.ReplaceAll("<@Producent>", Producent, SearchOptions.None);
                rec1.Document.ReplaceAll("<@PH_Producent>", PH_Producent, SearchOptions.None);
                rec1.Document.ReplaceAll("<@Numer_Zam>", Numer_Zam, SearchOptions.None);
                rec1.Document.ReplaceAll("<@Data_Otrz>", FileDate, SearchOptions.None);
                rec1.Document.ReplaceAll("<@Data_Zam>", ZamDate, SearchOptions.None);
                rec1.Document.ReplaceAll("<@Uwagi>", Uwagi, SearchOptions.None);
                rec1.Document.ReplaceAll("<@Rodzaj_Zam>", Rodzaj_zam, SearchOptions.None);
                rec1.Document.ReplaceAll("<@Knt_XLAkr>", XL_Akronim, SearchOptions.None);
                rec1.Document.ReplaceAll("<@Knt_ZamProd>", Prod_Akronim, SearchOptions.None);
                rec1.Document.ReplaceAll("<@Knt_Adres>", Prod_Adres, SearchOptions.None);

                DocumentRange[] wyniki= rec1.Document.FindAll("<@TABELA>", SearchOptions.None);
                if (wyniki.Length > 0)
                {
                   // Paragraph par = rec1.Document.Paragraphs.Get(wyniki[0].Start);
                    DocumentPosition pos = wyniki[0].Start;
                    SubDocument doc = pos.BeginUpdateDocument();


                    // Add the table
                    rec1.Document.Tables.Create(pos, 1, 9, AutoFitBehaviorType.AutoFitToContents);
                    // Format the table
                    Table tbl = rec1.Document.Tables[0];

                    pos.EndUpdateDocument(doc);
                    try
                    {
                        tbl.BeginUpdate();

                        CharacterProperties cp_Tbl = doc.BeginUpdateCharacters(tbl.Range);
                        cp_Tbl.FontSize = 10;
                        cp_Tbl.FontName = "Calibri";
                        doc.EndUpdateCharacters(cp_Tbl);
                        //tbl.BeginUpdate();

                        // Insert header caption and format the columns
                        tbl.Rows[0].HeightType = HeightType.Exact;
                        tbl.Rows[0].Height = 80f;
                        doc.InsertSingleLineText(tbl[0, 0].Range.Start, "Lp.");
                        doc.InsertSingleLineText(tbl[0, 1].Range.Start, "Alert");
                        doc.InsertSingleLineText(tbl[0, 2].Range.Start, "Kod");
                        doc.InsertSingleLineText(tbl[0, 3].Range.Start, "Towar");
                        doc.InsertSingleLineText(tbl[0, 4].Range.Start, "il. szt.");
                        doc.InsertSingleLineText(tbl[0, 5].Range.Start, "Ilość");
                        doc.InsertSingleLineText(tbl[0, 6].Range.Start, "JM");
                        doc.InsertSingleLineText(tbl[0, 7].Range.Start, "Cena");
                        doc.InsertSingleLineText(tbl[0, 8].Range.Start, "Gratis");
                        tbl[0, 0].PreferredWidthType = WidthType.Fixed;
                        tbl[0, 0].PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(0.2f);
                        tbl[0, 0].VerticalAlignment = TableCellVerticalAlignment.Center;
                        tbl[0, 1].PreferredWidthType = WidthType.Fixed;
                        tbl[0, 1].PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(0.4f);
                        tbl[0, 1].VerticalAlignment = TableCellVerticalAlignment.Center;
                        tbl[0, 2].PreferredWidthType = WidthType.Fixed;
                        tbl[0, 2].PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(0.8f);
                        tbl[0, 2].VerticalAlignment = TableCellVerticalAlignment.Center;
                        tbl[0, 3].PreferredWidthType = WidthType.Fixed;
                        tbl[0, 3].PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(2.6f);
                        tbl[0, 3].VerticalAlignment = TableCellVerticalAlignment.Center;
                        tbl[0, 4].PreferredWidthType = WidthType.Fixed;
                        tbl[0, 4].PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(0.5f);
                        tbl[0, 4].VerticalAlignment = TableCellVerticalAlignment.Center;
                        tbl[0, 5].PreferredWidthType = WidthType.Fixed;
                        tbl[0, 5].PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(0.6f);
                        tbl[0, 5].VerticalAlignment = TableCellVerticalAlignment.Center;
                        tbl[0, 6].PreferredWidthType = WidthType.Fixed;
                        tbl[0, 6].PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(0.3f);
                        tbl[0, 6].VerticalAlignment = TableCellVerticalAlignment.Center;
                        tbl[0, 7].PreferredWidthType = WidthType.Fixed;
                        tbl[0, 7].PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(0.6f);
                        tbl[0, 7].VerticalAlignment = TableCellVerticalAlignment.Center;
                        tbl[0, 8].PreferredWidthType = WidthType.Fixed;
                        tbl[0, 8].PreferredWidth = DevExpress.Office.Utils.Units.InchesToDocumentsF(0.4f);
                        tbl[0, 8].VerticalAlignment = TableCellVerticalAlignment.Center;
                        /*
                        //Apply formatting to the "Active Customers" cell
                        CharacterProperties properties = rec1.Document.BeginUpdateCharacters(tbl[0, 1].ContentRange);
                        properties.FontName = "Segoe UI";
                        properties.FontSize = 16;
                        document.EndUpdateCharacters(properties);
                        ParagraphProperties alignment = document.BeginUpdateParagraphs(table[0, 1].ContentRange);
                        alignment.Alignment = ParagraphAlignment.Center;
                        document.EndUpdateParagraphs(alignment);
                        table[0, 1].VerticalAlignment = TableCellVerticalAlignment.Center;
                        */

                        int wiersz = 0;
                        DataTable dt_poz = ZamowieniaPozycje_DoWyslaniaOK(Zpn_id).Result;
                        foreach (DataRow r_poz in dt_poz.Rows)
                        {
                            wiersz++;
                            tbl.Rows.InsertAfter(wiersz-1);
                            tbl.Rows[wiersz].HeightType = HeightType.Auto;
                            if (bool.TryParse(r_poz["Alert"].ToString(), out bool ale)) 
                            {
                                if (ale)
                                { doc.Images.Insert(tbl[wiersz, 1].Range.Start, DocumentImageSource.FromFile("alert.png")); }
                            } else { ale = false; }
                            doc.InsertSingleLineText(tbl[wiersz, 0].Range.Start, r_poz["Poz"].ToString());
                            doc.InsertSingleLineText(tbl[wiersz, 2].Range.Start, r_poz["Kod"].ToString());
                            doc.InsertSingleLineText(tbl[wiersz, 3].Range.Start, r_poz["Towar_MAG"].ToString());
                            doc.InsertSingleLineText(tbl[wiersz, 4].Range.Start, r_poz["Ilosc_szt"].ToString());
                            doc.InsertSingleLineText(tbl[wiersz, 5].Range.Start, r_poz["Ilosc_JM"].ToString());
                            doc.InsertSingleLineText(tbl[wiersz, 6].Range.Start, r_poz["JM"].ToString().ToUpper());
                            doc.InsertSingleLineText(tbl[wiersz, 7].Range.Start, r_poz["Cena_Netto"].ToString());
                            if (bool.TryParse(r_poz["Gratis"].ToString(), out bool gratt))
                            { }
                            else { gratt = false; }
                            if (bool.TryParse(r_poz["Promocja"].ToString(), out bool dol))
                            { }
                            else { ale = false; }

                            if (gratt || dol || r["Zpn_TYP_ZAM"].ToString() == "ZG")
                            { doc.Images.Insert(tbl[wiersz, 8].Range.Start, DocumentImageSource.FromFile("gift.png")); }

                        }
                        //Apply formatting to the header cells
                        CharacterProperties headerRowProperties = rec1.Document.BeginUpdateCharacters(tbl.Rows[0].Range);
                        headerRowProperties.FontName = "Calibri";
                        headerRowProperties.FontSize = 10;
                        headerRowProperties.Bold = true;
                        //headerRowProperties.ForeColor = Color.FromArgb(212, 236, 183);
                        rec1.Document.EndUpdateCharacters(headerRowProperties);

                        ParagraphProperties headerRowParagraphProperties = rec1.Document.BeginUpdateParagraphs(tbl.Rows[0].Range);
                        headerRowParagraphProperties.Alignment = ParagraphAlignment.Center;
                        float f = 0.4f;
                        headerRowParagraphProperties.LeftIndent = f;
                        headerRowParagraphProperties.SpacingBefore = 2;
                        headerRowParagraphProperties.SpacingAfter = 2;
                        rec1.Document.EndUpdateParagraphs(headerRowParagraphProperties);
                        //Apply formatting to Row cells
                        if (tbl.Rows.Count > 1)
                        {
                            DocumentRange targetRange = rec1.Document.CreateRange(tbl[1, 0].Range.Start, tbl[tbl.Rows.Count-1,8].Range.End.ToInt());
                            ParagraphProperties RowParagraphProperties = rec1.Document.BeginUpdateParagraphs(targetRange);
                            
                            RowParagraphProperties.LeftIndent = DevExpress.Office.Utils.Units.InchesToDocumentsF(0.1f); ;
                            RowParagraphProperties.SpacingBefore = 8;
                            RowParagraphProperties.SpacingAfter = 8;
                            rec1.Document.EndUpdateParagraphs(RowParagraphProperties);
                            CharacterProperties infoProperties = rec1.Document.BeginUpdateCharacters(targetRange);
                            infoProperties.FontSize = 10;
                            infoProperties.FontName = "Calibri";
                            
                            rec1.Document.EndUpdateCharacters(infoProperties);
                        }
                        //Apply formatting to Number cells
                        for (int i=1;i<tbl.Rows.Count;i++)
                        {
                            ParagraphProperties RrParagraphProperties = rec1.Document.BeginUpdateParagraphs(tbl[i,4].Range);
                            RrParagraphProperties.Alignment = ParagraphAlignment.Right;
                            rec1.Document.EndUpdateParagraphs(RrParagraphProperties);
                            RrParagraphProperties = rec1.Document.BeginUpdateParagraphs(tbl[i, 5].Range);
                            RrParagraphProperties.Alignment = ParagraphAlignment.Right;
                            rec1.Document.EndUpdateParagraphs(RrParagraphProperties);
                            RrParagraphProperties = rec1.Document.BeginUpdateParagraphs(tbl[i, 7].Range);
                            RrParagraphProperties.Alignment = ParagraphAlignment.Right;
                            rec1.Document.EndUpdateParagraphs(RrParagraphProperties);
                            RrParagraphProperties = rec1.Document.BeginUpdateParagraphs(tbl[i, 0].Range);
                            RrParagraphProperties.Alignment = ParagraphAlignment.Right;
                            rec1.Document.EndUpdateParagraphs(RrParagraphProperties);
                        }
                    }
                    finally
                    {
                        tbl.EndUpdate();
                        doc.BeginUpdate();
                        TableStyle tStyleMain = rec1.Document.TableStyles.CreateNew();

                        //Specify style options        
                        tStyleMain.TableBorders.InsideHorizontalBorder.LineStyle = TableBorderLineStyle.Single;
                        tStyleMain.TableBorders.InsideHorizontalBorder.LineColor = Color.White;

                        tStyleMain.TableBorders.InsideVerticalBorder.LineStyle = TableBorderLineStyle.Single;
                        tStyleMain.TableBorders.InsideVerticalBorder.LineColor = Color.White;
                        tStyleMain.CellBackgroundColor = Color.FromArgb(227, 238, 220);
                        tStyleMain.Name = "MyTableStyle";

                        //Add the style to the document collection 
                        rec1.Document.TableStyles.Add(tStyleMain);

                        //Create conditional styles (styles for specific table elements)          
                        TableConditionalStyle myNewStyleForOddRows = tStyleMain.ConditionalStyleProperties.CreateConditionalStyle(ConditionalTableStyleFormattingTypes.OddRowBanding);
                        myNewStyleForOddRows.CellBackgroundColor = Color.FromArgb(196, 220, 182);

                        TableConditionalStyle myNewStyleForBottomRightCell = tStyleMain.ConditionalStyleProperties.CreateConditionalStyle(ConditionalTableStyleFormattingTypes.BottomRightCell);
                        myNewStyleForBottomRightCell.CellBackgroundColor = Color.FromArgb(188, 214, 201);
                        doc.EndUpdate();

                        doc.BeginUpdate();

                        // Apply a previously defined style to the table 
                        tbl.Style = tStyleMain;
                        doc.EndUpdate();
                        rec1.Document.ReplaceAll("<@TABELA>", "", SearchOptions.None);


                    }
                }
                #endregion
                #region Wysyłanie Maila i update statusów
                if (r["prc_email"].ToString().Length > 0)
                {
                    string Temat = "Nowe zamówienie producenckie.";
                    try
                    {
                        MailMessage mailMessage = new MailMessage("raporty@mag.olsztyn.pl", r["prc_email"].ToString());
                        mailMessage.Subject = Temat;

                        RichEditMailMessageExporter exporter = new RichEditMailMessageExporter(rec1, mailMessage);
                        exporter.Export();

                        SmtpClient mailSender = new SmtpClient();
                        mailSender.Port = 587;
                        mailSender.Host = "mag-ol.home.pl";
                        mailSender.Timeout = 10000;
                        mailSender.DeliveryMethod = SmtpDeliveryMethod.Network;
                        mailSender.UseDefaultCredentials = false;
                        mailSender.Credentials = new NetworkCredential("raporty@mag.olsztyn.pl", "!Raporty123");
                        mailSender.EnableSsl = true;
                        mailMessage.From = new MailAddress("raporty@mag.olsztyn.pl");
                        //specify your login/password to log on to the SMTP server, if required
                        //mailSender.Credentials = new NetworkCredential("login", "password");
                        mailSender.Send(mailMessage);
                        int.TryParse(r["Zpn_ID"].ToString(), out int id);
                        if (id > 0)
                        {
                            SQL.UpdateDocumentMailStatus(id);
                        }
                    }
                    catch (Exception exc)
                    {
                        MessageBox.Show(exc.Message);

                    }
                }
                #endregion
            }


        }
        public  void PobierzKonfiguracje()
        {
            konfiguracja = ReturnConfig().Result;
        }
        public void WeryfikacjaZamowien()
        {
            using (var context = new CDNXL_MAGEntities(SQL.MAGConnectionString, true, true, false))
            {
                UpdateDocumentStatistics();
            }
        }
        public void GenerowaniePlikowMobius()
        {
            foreach (DataRow ZSRow in ExportMobiusList().Result.Rows)
            {
                string lokalizacja = ZSRow["EZam_FoldDoc"].ToString();
                if (Directory.Exists(lokalizacja))
                {
                    DataTable naglowki = ExportMobiusNag(Convert.ToInt32(ZSRow["Zpn_ID"])).Result;
                    List<string> Lines = new List<string>();
                    string linia = "";
                    foreach (DataColumn cn in naglowki.Columns)
                    {
                        if (cn.ColumnName != "Nazwa_Pliku")
                        {
                            linia += cn.ColumnName + "|";
                        }
                    }
                    Lines.Add(linia);
                    linia = "";
                    foreach (DataColumn cn in naglowki.Columns)
                    {
                        if (cn.ColumnName != "Nazwa_Pliku")
                        {
                            linia += naglowki.Rows[0][cn].ToString() + "|";
                        }
                    }
                    Lines.Add(linia);
                    linia = "";
                    DataTable pozycje = ExportMobiusPoz(Convert.ToInt32(ZSRow["Zpn_ID"])).Result;
                    foreach (DataColumn cn in pozycje.Columns)
                    {
                        linia += cn.ColumnName + "|";
                    }
                    Lines.Add(linia);
                    linia = "";
                    foreach (DataRow dr in pozycje.Rows)
                    {
                        foreach (DataColumn cn in pozycje.Columns)
                        {
                            linia += dr[cn].ToString() + "|";
                        }
                        Lines.Add(linia);
                        linia = "";

                    }
                    if (naglowki.Rows[0]["Nazwa_Pliku"].ToString().Length > 3)
                    {
                        using (StreamWriter streamWriter = new StreamWriter(Path.Combine(lokalizacja, naglowki.Rows[0]["Nazwa_Pliku"].ToString()),true,Encoding.UTF8))
                        {
                            foreach (string ln in Lines)
                            {
                                streamWriter.WriteLine(ln);
                            }
                        }
                    }
                }
            }
        }
        public void Importtest()
        {
            string sciezka_zrd = @"C:\Users\wojciech.bazydlo\Desktop\ebi\ebixwb";
            foreach (string plik in ListFolderFiles(sciezka_zrd, "*.ZAM"))
            {
                DataTable dt1= Asseco311_ReadFile(plik, 1); 
                DataTable dt2= Asseco311_ReadFile(plik, 2);
                Asseco311_DBSave(dt1, dt2, Path.GetFileName(plik), sciezka_zrd,"","",DateTime.Now);
            }
        }
        public void ImportASSECO311(string sciezka_zrd, string rozszerzenie,string sciezka_arch,string prodAkr, string prodNazwa)
        {
            if (Directory.Exists(sciezka_zrd))
            {
                foreach (string plik in ListFolderFiles(sciezka_zrd, rozszerzenie))
                {
                    DataTable dt1 = Asseco311_ReadFile(plik, 1);
                    DataTable dt2 = Asseco311_ReadFile(plik, 2);
                    Asseco311_DBSave(dt1, dt2, Path.GetFileName(plik), sciezka_zrd,prodAkr,prodNazwa,File.GetCreationTime(plik));
                    MoveFile(plik, sciezka_arch);
                }
            }
        }
        public List<string> Synergizer_ExportFileLines(DataTable Naglowki, DataTable Pozycje)
        {
            List<string> Wiersze = new List<string>();
            if (Naglowki!=null && Pozycje!=null)
            {
                Wiersze.Add(@"LP_ZAM|ZRODLO_CEL|ID_MAG|NR_ZAM|NR_ZAM_KLT|ILOSC_POZ|TYP_ZRODLA|ID_PRAC|NAZWISKO|IMIE|ID_KLT|NAZWA|MIASTO|ULICA|ID_CENNIK|TYP_ZAM|RODZ_DOK|DATA_ZAM|DATA_DOST|SPOSOB_DOST|ADRES_DOST|FORMA_PLT|TERMIN_PLT|RABAT_ZAM|WSK_PRM|ID_PRM_EBI|NAZWA_PRM|UWAGI|");
                foreach (DataRow r in Naglowki.Rows)
                {
                    Wiersze.Add(@"-1" + "|"
                    + r["ZRODLO_CEL"] + "|"
                    + r["ID_MAG"] + "|"
                    + r["NR_ZAM"] + "|"
                    + r["NR_ZAM"] + "|"
                    + Pozycje.Rows.Count.ToString() + "|"
                    + "1" + "|"//typ_zrodla 
                    + r["ID_PRAC"] + "|"
                    + r["NAZWISKO"] + "|"
                    + r["IMIE"] + "|"
                    + r["ID_KLT"] + "|"
                    + r["NAZWA"] + "|"
                    + r["MIASTO"] + "|"
                    + r["ULICA"] + "|"
                    + "1" + "|"
                    + "ZW" + "|"
                    + "FA" + "|"
                    + r["DATA_ZAM"] + "|"
                    + r["DATA_DOST"] + "|"
                    + "D" + "|"
                    + "" + "|"
                    + "P" + "|"
                    + "0" + "|"
                    + "0.00" + "|"
                    + r["WSK_PRM"] + "|"
                    + r["ID_PRM_EBI"] + "|"
                    + r["NAZWA_PRM"] + "|"
                    + r["UWAGI"] + "|");
                }
                Wiersze.Add(@"NR_POZ|ID_ASO|EAN|NAZWA|ILOSC_SZT|ILOSC|ID_JED|ILOSC_NZ|DATA_NZ|CENA_NETTO|CENA_NETTO_RABAT|RABAT|UKRYJ_RABAT|WSK_GRATIS|WSK_PRM|ID_PRM_EBI|ID_PRM_DYS|");
                foreach (DataRow r in Pozycje.Rows)
                {
                    Wiersze.Add(@""
                    + r["NR_POZ"] + "|"
                    + r["ID_ASO"] + "|"
                    + r["EAN"] + "|"
                    + r["NAZWA"] + "|"
                    + r["ILOSC_SZT"] + "|"
                    + r["ILOSC"] + "|"
                    + r["ID_JED"] + "|"
                    + "0" + "|"
                    + "" + "|"
                    + "0.00" + "|"
                    + "0.00" + "|"
                    + r["RABAT"] + "|"
                    + "0" + "|"
                    + "0" + "|"
                    + "0" + "|"
                    + "" + "|"
                    + "" + "|");

                }
            }
            return Wiersze;
        }
        public DataTable Asseco311_ReadFile(string sciezka, int typ) //typ: 1-zwróć nagłówek, 2-zwróć pozycje
        {
            if (typ == 1)
            {
                DataTable wynik = new DataTable("Naglowki");
                wynik.Columns.Add("ZRODLO_CEL", typeof(string));
                wynik.Columns.Add("ID_MAG", typeof(string));
                wynik.Columns.Add("NR_ZAM", typeof(string));
                wynik.Columns.Add("ID_PRAC", typeof(string));
                wynik.Columns.Add("NAZWISKO", typeof(string));
                wynik.Columns.Add("IMIE", typeof(string));
                wynik.Columns.Add("ID_KLT", typeof(string));
                wynik.Columns.Add("NAZWA", typeof(string));
                wynik.Columns.Add("MIASTO", typeof(string));
                wynik.Columns.Add("ULICA", typeof(string));
                wynik.Columns.Add("NR_LOK", typeof(string));
                wynik.Columns.Add("NIP", typeof(string));
                wynik.Columns.Add("TYP_ZAM", typeof(string));
                wynik.Columns.Add("DATA_ZAM", typeof(string));
                wynik.Columns.Add("DATA_DOST", typeof(string));
                wynik.Columns.Add("RABAT_ZAM", typeof(string));
                wynik.Columns.Add("UWAGI", typeof(string));
                wynik.Columns.Add("WSK_PRM", typeof(string));
                wynik.Columns.Add("ID_PRM_EBI", typeof(string));
                wynik.Columns.Add("NAZWA_PRM", typeof(string));
                if (File.Exists(sciezka))
                {
                    try
                    {
                        using (StreamReader streamReader = new StreamReader(sciezka, Encoding.GetEncoding("ISO-8859-2")))
                        {
                            string[] NaglowkiKolumny = streamReader.ReadLine().Split(new char[] { '|' });
                            string[] NaglowkiDane = streamReader.ReadLine().Split(new char[] { '|' });
                            DataRow row = wynik.NewRow();
                            for (int i = 0; i <= 19; i++)
                            {
                                row[i] = NaglowkiDane[i];
                            }
                            wynik.Rows.Add(row);
                        }
                        return wynik;
                    }
                    catch (Exception ex)
                    { 
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
            else
            {
                DataTable wynik = new DataTable("Pozycje");
                wynik.Columns.Add("NR_POZ", typeof(string));
                wynik.Columns.Add("ID_ASO", typeof(string));
                wynik.Columns.Add("EAN", typeof(string));
                wynik.Columns.Add("NAZWA", typeof(string));
                wynik.Columns.Add("ILOSC_SZT", typeof(string));
                wynik.Columns.Add("ILOSC", typeof(string));
                wynik.Columns.Add("ID_JED", typeof(string));
                wynik.Columns.Add("RABAT", typeof(string));
                if (File.Exists(sciezka))
                {
                    try
                    {
                        using (StreamReader streamReader = new StreamReader(sciezka, Encoding.GetEncoding("ISO-8859-2")))
                        {
                            streamReader.ReadLine();
                            streamReader.ReadLine();
                            streamReader.ReadLine();
                            while (!streamReader.EndOfStream)
                            {
                                string[] WierszPozycji = streamReader.ReadLine().Split(new char[] { '|' });
                                DataRow row = wynik.NewRow();
                                for (int i = 0; i <= 7; i++)
                                {
                                    row[i] = WierszPozycji[i];
                                }
                                wynik.Rows.Add(row);
                            }
                        }
                        return wynik;
                    }
                    catch (Exception ex)
                    {
                        return null;
                    }
                }
                else
                {
                    return null;
                }
            }
        }
        public void Asseco311_DBSave(DataTable Naglowki, DataTable Pozycje, string FName, string path, string prodAkr, string prodNazwa, DateTime filedate)
        {
            if (Naglowki != null && Pozycje != null)
            {
                if (Naglowki.Rows[0]["ZRODLO_CEL"].ToString()==prodAkr)
                {
                    DateTime.TryParse(Naglowki.Rows[0]["DATA_ZAM"].ToString(), out DateTime DataZam);
                    DateTime.TryParse(Naglowki.Rows[0]["DATA_DOST"].ToString(), out DateTime DataDost);
                    decimal.TryParse(Naglowki.Rows[0]["RABAT_ZAM"].ToString().Replace('.', ','), out decimal RabatZam);
                    int.TryParse(Naglowki.Rows[0]["WSK_PRM"].ToString(), out int wsk_prm);
                    int.TryParse(Naglowki.Rows[0]["ID_PRM_EBI"].ToString(), out int id_prm);
                    MAG_ZamProdNag nag=SQL.InsertNagZamNag(FName, path, prodNazwa, 0, false, false, 0, Naglowki.Rows[0]["ZRODLO_CEL"].ToString(), Naglowki.Rows[0]["ID_MAG"].ToString(), Naglowki.Rows[0]["NR_ZAM"].ToString()
                        , Naglowki.Rows[0]["ID_PRAC"].ToString(), Naglowki.Rows[0]["NAZWISKO"].ToString(), Naglowki.Rows[0]["IMIE"].ToString(), Naglowki.Rows[0]["ID_KLT"].ToString(), Naglowki.Rows[0]["NAZWA"].ToString()
                        , Naglowki.Rows[0]["MIASTO"].ToString(), Naglowki.Rows[0]["ULICA"].ToString(), Naglowki.Rows[0]["NR_LOK"].ToString(), Naglowki.Rows[0]["NIP"].ToString(), Naglowki.Rows[0]["TYP_ZAM"].ToString(),
                        DataZam, DataDost, RabatZam, Naglowki.Rows[0]["UWAGI"].ToString(), wsk_prm,
                        id_prm, Naglowki.Rows[0]["NAZWA_PRM"].ToString(),0,filedate);
                    if (Pozycje.Rows.Count > 0)
                    {
                        foreach (DataRow r in Pozycje.Rows)
                        {
                            int.TryParse(r["NR_POZ"].ToString(), out int nr_poz);
                            decimal.TryParse(r["ILOSC_SZT"].ToString().Replace('.',','), out decimal il_szt);
                            decimal.TryParse(r["ILOSC"].ToString().Replace('.', ','), out decimal ilosc);
                            decimal.TryParse(r["RABAT"].ToString().Replace('.', ','), out decimal rabat);
                            SQL.InsertNagZamPoz(nag.Zpn_ID, false, 0, nr_poz, r["ID_ASO"].ToString(), r["EAN"].ToString(), r["NAZWA"].ToString(), il_szt, ilosc, r["ID_JED"].ToString(), null, null, null, null, rabat, null, null, null, null, null);
                        }
                    }
                }
            }
        }
        public void SaveFile(List<string> linie, string sciezka)
        {
            if (Directory.Exists(Path.GetDirectoryName(sciezka)) && linie.Count>0)
            {
                using (StreamWriter streamWriter = new StreamWriter(sciezka, false, Encoding.UTF8))
                {
                    foreach (string value2 in linie)
                    {
                        streamWriter.WriteLine(value2);
                    }
                }
            }
            else
            {
                MessageBox.Show(sciezka);
            }
        }
        public void MoveFile(string sciezka_Plik, string sciezka_docelowa)
        {
            try
            {
                if (Directory.Exists(sciezka_docelowa) && File.Exists(sciezka_Plik))
                {
                    string sciezka_PlikDocelowy = sciezka_docelowa + @"\" + Path.GetFileName(sciezka_Plik);
                    File.Move(sciezka_Plik, sciezka_PlikDocelowy);
                }
            }  
            catch (IOException iox)  
            {  
                Console.WriteLine(iox.Message);  
            }
        }
        public List<string> ListFolderFiles(string sciezka, string rozszerzenie)
        {
            List<string> lista_plikow = new List<string>();
            if (Directory.Exists(sciezka))
            {
                foreach (string s in Directory.GetFiles(sciezka, rozszerzenie))
                {
                    lista_plikow.Add(s);
                }
            }
            return lista_plikow;
        }

    }
}
