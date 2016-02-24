using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Imaging;
using System.Xml;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
using Microsoft.Dexterity.Applications.DynamicsDictionary;
using HtmlAgilityPack;
using TheArtOfDev.HtmlRenderer.WinForms;
using System.Globalization;

namespace KursPajakOnline
{
    class KursLine
    {
        public string No;
        public string MataUang;
        public string Negara;
        public string NegaraISO;
        public string Nilai;
        public string Perubahan;
    }

    public class GPAddIn : IDexterityAddIn
    {
        // IDexterityAddIn interface

        static McExchangeRateMaintenanceForm MCExchRateMaintForm = Dynamics.Forms.McExchangeRateMaintenance;

        static McExchangeRateMaintenanceForm.McExchangeRateMaintenanceWindow MCExchRateMaintWindow = MCExchRateMaintForm.McExchangeRateMaintenance;

        static McExchangeRateMstrTable MCExchangeRateMstrTable = Dynamics.Tables.McExchangeRateMstr;

        public void Initialize()
        {
            MCExchRateMaintForm.AddMenuHandler(getRateOnline, "Kurs KMK");
        }

        void getRateOnline(object sender, EventArgs e)
        {
            string html = "", htmlTable = "", message = "", tglKurs = "";
            int dataPosition = 0, dataPosition2 = 0, dataPosition3 = 0;
            DateTime tglBerlaku, tglExpire, defaultGPDate = new DateTime(1900, 1, 1);
            TableError err;

            HtmlAgilityPack.HtmlWeb web = new HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load("http://www.fiskal.depkeu.go.id/dw-kurs-db.asp");
            HtmlAgilityPack.HtmlNodeCollection nodes1 = doc.DocumentNode.SelectNodes("//*[@id='default-divone']");
            HtmlAgilityPack.HtmlNodeCollection nodes2 = doc.DocumentNode.SelectNodes("//*[@id='default-divone']/div[2]");
            
            message = "";
            foreach (HtmlAgilityPack.HtmlNode node in nodes1)
            {
                message += node.InnerHtml;
                
            }

            html = Regex.Replace(message, @"\s+", " ");
            dataPosition = html.IndexOf("<p class=\"jud-kaji\">Data Kurs Pajak Tanggal");
            if(dataPosition == -1) dataPosition = 0;

            Image image = TheArtOfDev.HtmlRenderer.WinForms.HtmlRender.RenderToImage(html.Substring(dataPosition));

            System.IO.Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\KursPajak");

            image.Save(Environment.GetFolderPath(Environment.SpecialFolder.Personal) + "\\KursPajak\\akses-" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".png", ImageFormat.Png);

            dataPosition = html.IndexOf("<p>Tanggal Berlaku");
            dataPosition2 = html.IndexOf("</p> </div> <div class=\"col-xs-4\"> <a");
            tglKurs = html.Substring(dataPosition, dataPosition2-dataPosition);
            dataPosition3 = tglKurs.IndexOf("-");

            CultureInfo idCulture = new CultureInfo("id-id", false);
            tglBerlaku = DateTime.Parse(tglKurs.Substring(21, dataPosition3 - 22), idCulture);
            tglExpire = DateTime.Parse(tglKurs.Substring(dataPosition3 + 2, tglKurs.Length - dataPosition3 - 7), idCulture);

            //MessageBox.Show("Tgl Berlaku = " + tglBerlaku.ToString("dd MMMM yyyy"));
            //MessageBox.Show("Tgl Expire = " + tglExpire.ToString("dd MMMM yyyy"));

            //html.IndexOf()
            //tglBerlaku = DateTime.Par
            message = "";
            foreach (HtmlAgilityPack.HtmlNode node in nodes2)
            {
                message = node.InnerHtml;
                
            }
            htmlTable = Regex.Replace(message, @"\s+", " ").Replace("<img src='data/aimages/down.gif'>", "").Replace("<img src='data/aimages/up.gif'>", ""); //clean up whitespace and img tag
            //MessageBox.Show(htmlTable);
            
            
            XmlDocument xmlTable = new XmlDocument();
            xmlTable.LoadXml(htmlTable);

            XmlNodeList trLines = xmlTable.DocumentElement.SelectNodes("/table/tr");

            //List<KursLine> kurs = new List<KursLine>();
            message = "";
            message += "Tanggal Berlaku = " + tglBerlaku.ToString("dd MMMM yyyy") + Environment.NewLine;
            message += "Tanggal Expire = " + tglExpire.ToString("dd MMMM yyyy") + Environment.NewLine;
            foreach (XmlNode trLine in trLines)
            {
                Boolean notUsed = false;
                //<tr><td class='text-left'>1</td><td class='text-left'>Dolar</td><td>Amerika Serikat (USD)</td><td class='text-right'>13,816.00 </td><td class='text-right'>0.43 %</td></tr>

                XmlNodeList tdLines = trLine.SelectNodes("td");
                int i = 1;
                KursLine kur = new KursLine();
                foreach (XmlNode tdLine in tdLines)
                {

                    switch (i)
                    {
                        case 1:
                            kur.No = tdLine.InnerText.Trim();
                            break;
                        case 2:
                            kur.MataUang = tdLine.InnerText.Trim();
                            break;
                        case 3:
                            kur.Negara = tdLine.InnerText.Trim();
                            notUsed = false;
                            switch (kur.Negara)//no use going forward if not interested
                            {
                                case "Amerika Serikat (USD)":
                                    kur.NegaraISO = "USD";
                                    break;
                                case "India (INR)":
                                    kur.NegaraISO = "INR";
                                    break;
                                case "Singapura (SGD)":
                                    kur.NegaraISO = "SGD";
                                    break;
                                case "Euro (EUR)":
                                    kur.NegaraISO = "EUR";
                                    break;
                                default:
                                    notUsed = true;
                                    break;
                            }
                            break;
                        case 4:
                            kur.Nilai = tdLine.InnerText.Trim();
                            break;
                        case 5:
                            kur.Perubahan = tdLine.InnerText.Trim();
                            break;
                        default:
                            break;
                    }
                    if (notUsed) break; //if not the interested currency, then continue next trline
                    i++;
                }
                if (notUsed) continue;
                //at this point we should have an interested xchange rate
                //MessageBox.Show(kur.Negara + " " + double.Parse(kur.Nilai, new CultureInfo("en-us")));

                //add table insert logic here
                //first check if this week xchange rate is already input by user, we should not override user value


                MCExchangeRateMstrTable.Key = 1;
                MCExchangeRateMstrTable.ExchangeTableId.Value = kur.NegaraISO + "-TAX";
                MCExchangeRateMstrTable.ExchangeDate.Value = tglBerlaku;
                MCExchangeRateMstrTable.Time.Value = defaultGPDate;
                
                err = MCExchangeRateMstrTable.Get();
                if (err == TableError.NoError)
                {
                    //found
                    MessageBox.Show("Previously inputted exchange rate found for " + kur.Negara + Environment.NewLine + "Please confirm manually");
                    
                }
                else
                {
                    //if not inputted, then we insert the values
                    MCExchangeRateMstrTable.Clear();
                    MCExchangeRateMstrTable.Key = 1;
                    MCExchangeRateMstrTable.CurrencyId.Value = kur.NegaraISO;
                    MCExchangeRateMstrTable.ExchangeTableId.Value = kur.NegaraISO + "-TAX";
                    MCExchangeRateMstrTable.ExchangeDate.Value = tglBerlaku;
                    MCExchangeRateMstrTable.Time.Value = defaultGPDate;
                    MCExchangeRateMstrTable.ExchangeRate.Value = decimal.Parse(kur.Nilai, new CultureInfo("en-us"));
                    MCExchangeRateMstrTable.ExpirationDate.Value = tglExpire;
                    MCExchangeRateMstrTable.Save();
                }

                MCExchangeRateMstrTable.Close();
                message += kur.NegaraISO + "-TAX = " + decimal.Parse(kur.Nilai, new CultureInfo("en-us")) + Environment.NewLine;
            }//each trLines

            Form form = new Form();

            //Bitmap img = tpv.Img();

            form.StartPosition = FormStartPosition.CenterScreen;
            form.Size = image.Size;

            PictureBox pb = new PictureBox();
            pb.Dock = DockStyle.Fill;
            pb.Image = image;

            form.Controls.Add(pb);
            form.Show();
            form.Left += 450;

            message += "Please compare the values here with the image and then confirm in GP";
            MessageBox.Show(message);

            MCExchRateMaintWindow.TopOfFileButtonToolbar.RunValidate();
        }
    }
}
