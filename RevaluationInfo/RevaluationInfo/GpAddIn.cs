using System;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
using Microsoft.Dexterity.Applications.DynamicsDictionary;

namespace RevaluationInfo
{
    public class GPAddIn : IDexterityAddIn
    {
        // IDexterityAddIn interface
        static CmCheckbookBalanceForm CMCheckbookBalanceForm = Dynamics.Forms.CmCheckbookBalance;

        static CmCheckbookBalanceForm.CmCheckbookBalanceWindow CMCheckbookBalanceWindow = CMCheckbookBalanceForm.CmCheckbookBalance;

        //static CmCheckbookMstrTable CMCheckbookMstrTable = Dynamics.Tables.CmCheckbookMstr;
        static McExchangeRateMstrTable MCExchangeRateMstrTable = Dynamics.Tables.McExchangeRateMstr;

        public void Initialize()
        {
            CMCheckbookBalanceWindow.CheckbookId.Change += CheckbookId_Change;
        }

        void CheckbookId_Change(object sender, EventArgs e)
        {
            DateTime defaultGPDate = new DateTime(1900, 1, 1);
            TableError err;
            decimal currBalance;
            byte backDate = 1;
            Boolean XchRatefound = false;

            currBalance = CMCheckbookBalanceForm.Tables.CmCheckbookMstr.CurrentBalance.Value;

            //this is hardcoded value that may not be portable to other GP Setup
            //currencyid suffix is -TAX
            
            MCExchangeRateMstrTable.Clear();
            MCExchangeRateMstrTable.Key = 1;
            MCExchangeRateMstrTable.ExchangeTableId.Value = CMCheckbookBalanceForm.Tables.CmCheckbookMstr.CurrencyId.Value + "-TAX";
            MCExchangeRateMstrTable.ExchangeDate.Value = DateTime.Now.Date;
            MCExchangeRateMstrTable.Time.Value = defaultGPDate;

            err = MCExchangeRateMstrTable.Get();
            while (backDate < 7 & err == TableError.NotFound)//loop 7 times while notfound
            {
                MCExchangeRateMstrTable.Clear();
                MCExchangeRateMstrTable.Key = 1;
                MCExchangeRateMstrTable.ExchangeTableId.Value = CMCheckbookBalanceForm.Tables.CmCheckbookMstr.CurrencyId.Value + "-TAX";
                MCExchangeRateMstrTable.ExchangeDate.Value = DateTime.Now.Date.AddDays(backDate * -1);
                MCExchangeRateMstrTable.Time.Value = defaultGPDate;
                err = MCExchangeRateMstrTable.Get();
                if (err == TableError.NoError)//found
                {
                    XchRatefound = true;
                    break;
                }
                backDate++;
            }
            try
            {
                if (XchRatefound)
                {
                    Microsoft.Dexterity.Applications.DynamicsModified.Forms.CmCheckbookBalance.CmCheckbookBalance.LocalTglMulai.Value = MCExchangeRateMstrTable.ExchangeDate.Value;
                    Microsoft.Dexterity.Applications.DynamicsModified.Forms.CmCheckbookBalance.CmCheckbookBalance.LocalTglExpire.Value = MCExchangeRateMstrTable.ExpirationDate.Value;
                    Microsoft.Dexterity.Applications.DynamicsModified.Forms.CmCheckbookBalance.CmCheckbookBalance.LocalKursPajak.Value = MCExchangeRateMstrTable.ExchangeRate.Value;
                    Microsoft.Dexterity.Applications.DynamicsModified.Forms.CmCheckbookBalance.CmCheckbookBalance.LocalBalanceKurs.Value = MCExchangeRateMstrTable.ExchangeRate.Value * currBalance;
                }
                else
                {
                    Microsoft.Dexterity.Applications.DynamicsModified.Forms.CmCheckbookBalance.CmCheckbookBalance.LocalTglMulai.Value = DateTime.Now.Date;
                    Microsoft.Dexterity.Applications.DynamicsModified.Forms.CmCheckbookBalance.CmCheckbookBalance.LocalTglExpire.Value = DateTime.Now.Date;
                    Microsoft.Dexterity.Applications.DynamicsModified.Forms.CmCheckbookBalance.CmCheckbookBalance.LocalKursPajak.Value = 1;
                    Microsoft.Dexterity.Applications.DynamicsModified.Forms.CmCheckbookBalance.CmCheckbookBalance.LocalBalanceKurs.Value = 1 * currBalance;
                }
            }
            catch (Microsoft.Dexterity.Bridge.DexterityException ex)
            {
                MessageBox.Show(ex.Message + System.Environment.NewLine + "Are you using the Modified Window?");
            }
            //CMCheckbookMstrTable.Close();
            MCExchangeRateMstrTable.Close();
        }
    }
}
