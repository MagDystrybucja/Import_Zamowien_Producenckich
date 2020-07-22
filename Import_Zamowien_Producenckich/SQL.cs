using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;

namespace Import_Zamowien_Producenckich
{
    public static class SQL
    {
        public static string MAGConnectionString = @"Persist Security Info=False;Integrated Security=true;Initial Catalog=CDNXL_MAG;Server=mag-sql";
        public static MAG_ZamProdNag InsertNagZamNag(string _Zpn_nazwa_pliku, string _Zpn_Sciezka, string _Zpn_Producent, int _Zpn_Status, bool _Zpn_Alert,bool _Zpn_AlertKnt,int _Zpn_KntXL
                                            ,string _Zpn_ZRODLO_CEL,string _Zpn_ID_MAG, string _Zpn_NR_ZAM, string _Zpn_ID_PRAC, string _Zpn_NAZWISKO, string _Zpn_IMIE, string _Zpn_ID_KLT
                                            ,string _Zpn_NAZWA, string _Zpn_MIASTO, string _Zpn_ULICA, string _Zpn_NR_LOK, string _Zpn_NIP, string _Zpn_TYP_ZAM, DateTime _Zpn_DATA_ZAM
                                            ,DateTime _Zpn_DATA_DOST, Decimal _Zpn_RABAT_ZAM, string _Zpn_UWAGI, int _Zpn_WSK_PRM, int _Zpn_ID_PRM_EBI, string _Zpn_NAZWA_PRM, int _Zpn_EmailSent, DateTime filedate)
        {
                using (CDNXL_MAGEntities context = new CDNXL_MAGEntities(MAGConnectionString, true, true, false))
                {
                MAG_ZamProdNag newElement = new MAG_ZamProdNag()
                {
                    Zpn_nazwa_pliku = _Zpn_nazwa_pliku,
                    Zpn_Sciezka = _Zpn_Sciezka,
                    Zpn_Producent = _Zpn_Producent,
                    Zpn_Status = _Zpn_Status,
                    Zpn_Alert = _Zpn_Alert,
                    Zpn_AlertKnt = _Zpn_AlertKnt,
                    Zpn_KntXL = _Zpn_KntXL,
                    Zpn_ZRODLO_CEL = _Zpn_ZRODLO_CEL,
                    Zpn_ID_MAG = _Zpn_ID_MAG, 
                    Zpn_NR_ZAM = _Zpn_NR_ZAM,
                    Zpn_ID_PRAC = _Zpn_ID_PRAC,
                    Zpn_NAZWISKO = _Zpn_NAZWISKO,
                    Zpn_IMIE = _Zpn_IMIE,
                    Zpn_ID_KLT = _Zpn_ID_KLT,
                    Zpn_NAZWA = _Zpn_NAZWA,
                    Zpn_MIASTO = _Zpn_MIASTO,
                    Zpn_ULICA = _Zpn_ULICA,
                    Zpn_NR_LOK = _Zpn_NR_LOK,
                    Zpn_NIP = _Zpn_NIP,
                    Zpn_TYP_ZAM = _Zpn_TYP_ZAM,
                    Zpn_DATA_ZAM = _Zpn_DATA_ZAM,
                    Zpn_DATA_DOST = _Zpn_DATA_DOST,
                    Zpn_RABAT_ZAM = _Zpn_RABAT_ZAM,
                    Zpn_UWAGI = _Zpn_UWAGI,
                    Zpn_WSK_PRM = _Zpn_WSK_PRM,
                    Zpn_ID_PRM_EBI = _Zpn_ID_PRM_EBI,
                    Zpn_NAZWA_PRM = _Zpn_NAZWA_PRM,
                    Zpn_EmailSent = _Zpn_EmailSent,
                    Zpn_DataPliku = filedate
                };
                context.MAG_ZamProdNag.Add(newElement);
                context.SaveChanges();
                return newElement;

            }
        }
        public static MAG_ZamProdPoz InsertNagZamPoz(int _Zpp_zpnid,bool _Zpp_AlertTwr,int _Zpp_TwrGIDXL,int _Zpp_NR_POZ,string _Zpp_ID_ASO, string _Zpp_EAN, string _Zpp_NAZWA, decimal _Zpp_ILOSC_SZT
                                                    ,decimal _Zpp_ILOSC, string _Zpp_ID_JED, decimal? _Zpp_ILOSC_NZ, DateTime? _Zpp_DATA_NZ, decimal? _Zpp_CENA_NETTO, decimal? _Zpp_CENA_NETTO_RABAT
                                                    ,decimal? _Zpp_RABAT, int? _Zpp_UKRYJ_RABAT, int? _Zpp_WSK_GRATIS, int? _Zpp_WSK_PRM, int? _Zpp_ID_PRM_EBI, int? _Zpp_ID_PRM_DYS)
        {
            using (CDNXL_MAGEntities context = new CDNXL_MAGEntities(MAGConnectionString, true, true, false))
            {
                MAG_ZamProdPoz newElement = new MAG_ZamProdPoz()
                {
                    Zpp_zpnid = _Zpp_zpnid,
                    Zpp_AlertTwr = _Zpp_AlertTwr,
                    Zpp_TwrGIDXL = _Zpp_TwrGIDXL,
                    Zpp_NR_POZ = _Zpp_NR_POZ,
                    Zpp_ID_ASO = _Zpp_ID_ASO,
                    Zpp_EAN = _Zpp_EAN,
                    Zpp_NAZWA = _Zpp_NAZWA,
                    Zpp_ILOSC_SZT = _Zpp_ILOSC_SZT,
                    Zpp_ILOSC = _Zpp_ILOSC,
                    Zpp_ID_JED = _Zpp_ID_JED,
                    Zpp_ILOSC_NZ = _Zpp_ILOSC_NZ,
                    Zpp_DATA_NZ = _Zpp_DATA_NZ,
                    Zpp_CENA_NETTO = _Zpp_CENA_NETTO,
                    Zpp_CENA_NETTO_RABAT = _Zpp_CENA_NETTO_RABAT,
                    Zpp_RABAT = _Zpp_RABAT,
                    Zpp_UKRYJ_RABAT = _Zpp_UKRYJ_RABAT,
                    Zpp_WSK_GRATIS = _Zpp_WSK_GRATIS,
                    Zpp_WSK_PRM = _Zpp_WSK_PRM,
                    Zpp_ID_PRM_EBI = _Zpp_ID_PRM_EBI,
                    Zpp_ID_PRM_DYS = _Zpp_ID_PRM_DYS
                };
                context.MAG_ZamProdPoz.Add(newElement);
                context.SaveChanges();
                return newElement;

            }
        }

        public static Task<List<MAG_EBIZAM_Konfig>> ReturnConfig()
        {
            return Task.Run(() =>
            {
                using (CDNXL_MAGEntities context = new CDNXL_MAGEntities(MAGConnectionString, true, true, false))
                {
                    return context.MAG_EBIZAM_Konfig.Where(x => x.EZam_Archiwalny == false).ToList();
                }
            });
        }

        public static void UpdateDocumentStatistics()
        {
            using (var context = new CDNXL_MAGEntities(MAGConnectionString, true, true, false))
            {
                try
                {
                    context.Database.ExecuteSqlCommand("EXEC [CDN].[MAG_EDIZam_WeryfikacjaZamowien]");
                }
                catch (Exception ಠ_ಠ)
                {
                }
            }
        }

        public static void UpdateDocumentMailStatus(int id)
        {
            using (var context = new CDNXL_MAGEntities(MAGConnectionString, true, true, false))
            {
                try
                {
                    
                    context.Database.ExecuteSqlCommand(" UPDATE [CDN].[MAG_ZamProdNag] SET [Zpn_EmailSent]=1 where zpn_id="+id.ToString()) ;
                }
                catch (Exception ಠ_ಠ)
                {
                }
            }
        }

        public static Task<DataTable> Zamowienia_DoWyslaniaOK()
        {
            return Task.Run(() =>
            {
                DataTable zam = new DataTable();
                using (SqlConnection connection = new SqlConnection(MAGConnectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter(@"SELECT [Zpn_ID] ,[Zpn_nazwa_pliku] ,[Zpn_Sciezka] ,Zpn_DataPliku ,[Zpn_Producent] ,[Zpn_Status] ,[Zpn_Alert] ,[Zpn_AlertKnt] ,[Zpn_KntXL] ,[Zpn_EmailSent] ,[Zpn_ZRODLO_CEL] ,[Zpn_ID_MAG] ,[Zpn_NR_ZAM] ,[Zpn_ID_PRAC] ,[Zpn_NAZWISKO] ,[Zpn_IMIE] ,[Zpn_ID_KLT] ,[Zpn_NAZWA] ,[Zpn_MIASTO] ,[Zpn_ULICA] ,[Zpn_NR_LOK] ,[Zpn_NIP] ,[Zpn_TYP_ZAM] ,[Zpn_DATA_ZAM] ,[Zpn_DATA_DOST] ,[Zpn_RABAT_ZAM] ,[Zpn_UWAGI] ,[Zpn_WSK_PRM] ,[Zpn_ID_PRM_EBI] ,[Zpn_NAZWA_PRM],Knt_Akronim,MAG_Przedstawiciele.prc_email FROM [CDNXL_MAG].[CDN].[MAG_ZamProdNag] inner join CDNXL_MAG.CDN.KntKarty on Knt_GIDNumer=Zpn_KntXL inner join CDNXL_MAG.CDN.MAG_Przedstawiciele on Prc_GIDNumer=Knt_AkwNumer and Prc_GIDTyp=Knt_AkwTyp where Zpn_EmailSent=0 and Zpn_AlertKnt=0 ", connection))
                {
                    connection.Open();
                    adapter.Fill(zam);
                }
                return zam;
            });
        }
        public static Task<DataTable> ZamowieniaPozycje_DoWyslaniaOK(int Id)
        {
            return Task.Run(() =>
            {
                DataTable zam = new DataTable();
                using (SqlConnection connection = new SqlConnection(MAGConnectionString))
                using (SqlCommand command = new SqlCommand(@"SELECT ROW_NUMBER() OVER (Order by [Zpp_NR_POZ] asc) as Poz ,[Zpp_AlertTwr] as Alert ,[Zpp_NR_POZ] as Poz_zpp ,[Zpp_ID_ASO] as Kod ,[Zpp_NAZWA] as Towar_Nazwa ,Twr_Nazwa as Towar_MAG ,floor(isnull([Zpp_ILOSC_SZT],0)) as Ilosc_szt ,[Zpp_ILOSC] as Ilosc_JM ,[Zpp_ID_JED] as JM ,cast(isnull([Zpp_CENA_NETTO_RABAT],0.00) as decimal(12,2)) as Cena_Netto ,isnull(Zpp_WSK_GRATIS,0) as Gratis ,isnull([Zpp_WSK_PRM],0) as Promocja FROM [CDNXL_MAG].[CDN].[MAG_ZamProdPoz] left join CDNXL_MAG.CDN.TwrKarty on Twr_GIDNumer=Zpp_TwrGIDXL where Zpp_zpnid=@id ", connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = Id;
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        connection.Open();
                        adapter.Fill(zam);
                    }
                }
                return zam;
            });
        }
        public static Task<DataTable> ExportMobiusList()
        {
            return Task.Run(() =>
            {
                DataTable zam = new DataTable();
                using (SqlConnection connection = new SqlConnection(MAGConnectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter(@"Select Zpn_ID, EZam_FoldDoc FROM [CDNXL_MAG].[CDN].[MAG_ZamProdNag] inner join CDNXL_MAG.CDN.MAG_EBIZAM_Konfig on EZam_ProdAkr=Zpn_ZRODLO_CEL where Zpn_Status=1 and EZam_Archiwalny=0", connection))
                {
                    connection.Open();
                    adapter.Fill(zam);
                }
                return zam;
            });
        }
        public static Task<DataTable> ExportMobiusNag(int zpn_id)
        {
            return Task.Run(() =>
            {
                DataTable zam = new DataTable();
            using (SqlConnection connection = new SqlConnection(MAGConnectionString))
            using (SqlDataAdapter adapter = new SqlDataAdapter(@"Select Zpn_nazwa_pliku as Nazwa_Pliku, '-1' as LP_ZAM ,Zpn_ZRODLO_CEL as ZRODLO_CEL ,[Zpn_ID_MAG] as ID_MAG ,[Zpn_NR_ZAM] as NR_ZAM ,[Zpn_NR_ZAM] as NR_ZAM_KLT ,count(Zpp_ID) as ILOSC_POZ ,1 as TYP_ZRODLA ,[Zpn_ID_PRAC] as ID_PRAC ,[Zpn_NAZWISKO] as NAZWISKO ,[Zpn_IMIE] as IMIE ,[Zpn_ID_KLT] as ID_KLT ,[Zpn_NAZWA] as NAZWA ,[Zpn_MIASTO] as MIASTO ,[Zpn_ULICA] as ULICA ,0 as ID_CENNIK ,[Zpn_TYP_ZAM] as TYP_ZAM ,'FA' as RODZ_DOK ,Zpn_DATA_ZAM as DATA_ZAM ,Zpn_DATA_DOST as DATA_DOST ,'D' as SPOSOB_DOST , '' as ADRES_DOST ,'P' as FORMA_PLT , 0 as TERMIN_PLT ,Zpn_RABAT_ZAM as RABAT_ZAM ,Zpn_WSK_PRM as WSK_PRM ,Zpn_ID_PRM_EBI as ID_PRM_EBI ,Zpn_NAZWA_PRM as NAZWA_PRM ,Zpn_UWAGI as UWAGI FROM [CDNXL_MAG].[CDN].[MAG_ZamProdNag] inner join CDNXL_MAG.CDN.MAG_ZamProdPoz on Zpn_ID=Zpp_zpnid where Zpn_ID=" + zpn_id.ToString() + " group by Zpn_ZRODLO_CEL ,[Zpn_ID_MAG] ,[Zpn_NR_ZAM] ,[Zpn_NR_ZAM] ,[Zpn_ID_PRAC] ,[Zpn_NAZWISKO] ,[Zpn_IMIE] ,[Zpn_ID_KLT] ,[Zpn_NAZWA] ,[Zpn_MIASTO] ,[Zpn_ULICA] ,[Zpn_TYP_ZAM] ,Zpn_DATA_ZAM ,Zpn_DATA_DOST ,Zpn_RABAT_ZAM ,Zpn_WSK_PRM ,Zpn_ID_PRM_EBI ,Zpn_NAZWA_PRM ,Zpn_UWAGI,Zpn_nazwa_pliku ", connection))
                {
                    connection.Open();
                    adapter.Fill(zam);
                }
                return zam;
            });
        }
        public static Task<DataTable> ExportMobiusPoz(int zpn_id)
        {
            return Task.Run(() =>
            {
                DataTable zam = new DataTable();
                using (SqlConnection connection = new SqlConnection(MAGConnectionString))
                using (SqlDataAdapter adapter = new SqlDataAdapter(@"SELECT [Zpp_NR_POZ] as NR_POZ ,[Zpp_ID_ASO] as ID_ASO ,[Zpp_EAN] as EAN ,[Zpp_NAZWA] as NAZWA ,[Zpp_ILOSC_SZT] as ILOSC_SZT ,[Zpp_ILOSC] as ILOSC ,[Zpp_ID_JED] as ID_JED ,isnull([Zpp_ILOSC_NZ],0) as ILOSC_NZ ,isnull([Zpp_DATA_NZ],'') as DATA_NZ ,isnull([Zpp_CENA_NETTO],0.00) as CENA_NETTO ,isnull([Zpp_CENA_NETTO_RABAT],0.00) as CENA_NETTO_RABAT ,isnull([Zpp_RABAT],0.00) as RABAT ,isnull([Zpp_UKRYJ_RABAT],0) as UKRYJ_RABAT ,isnull([Zpp_WSK_GRATIS],0) as WSK_GRATIS ,isnull([Zpp_WSK_PRM],0) as WSK_PRM ,isnull([Zpp_ID_PRM_EBI],0) as ID_PRM_EBI ,isnull([Zpp_ID_PRM_DYS],0) as ID_PRM_DYS FROM [CDNXL_MAG].[CDN].[MAG_ZamProdPoz] where Zpp_zpnid= "+zpn_id.ToString(), connection))
                {
                    connection.Open();
                    adapter.Fill(zam);
                }
                return zam;
            });
        }
    }
}
