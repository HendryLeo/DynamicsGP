using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
using System.Windows.Forms;
using Microsoft.Dexterity.Applications.DynamicsDictionary;

namespace CompanyOptions
{
    public class GPAddIn : IDexterityAddIn
    {
        // IDexterityAddIn interface
        static SwitchCompanyForm SwitchCompanyForm = Microsoft.Dexterity.Applications.Dynamics.Forms.SwitchCompany;

        static SwitchCompanyForm.SwitchCompanyWindow SwitchCompanyWindow = SwitchCompanyForm.SwitchCompany;

        static SyCurrentActivityTable ActivityTable = Microsoft.Dexterity.Applications.Dynamics.Tables.SyCurrentActivity;
        
        public void Initialize()
        {
            // Register Event to trigger when Switch Company form opens
            SwitchCompanyWindow.OpenBeforeOriginal += new System.ComponentModel.CancelEventHandler(SwitchCompanyWindow_OpenBeforeOriginal);
        }

        void SwitchCompanyWindow_OpenBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        {
            List<string> users;
            TableError err;

            Dynamics.Application CompilerApp = new Dynamics.Application();
            string CompilerMessage = "";
            int CompilerError = 0;
            // Execute SanScript in Dynamics GP
            CompilerError = CompilerApp.ExecuteSanscript(Dexterity.CompanySwitchScript, out CompilerMessage);
            if (CompilerError != 0)
            {
                MessageBox.Show(CompilerMessage);
            }

            users = new List<string>();
            users.Clear();

            err = ActivityTable.GetFirst();
            while (err == TableError.NoError)
            {
                users.Add(ActivityTable.UserId.Value);
                err = ActivityTable.GetNext();
            }
            ActivityTable.Close();

            Boolean firstOne = true;
            //create string scrolling procedure here to display all user activity, otherwise Status is limited to 62 char
            foreach (string str in users.ToArray())
            {
                if (firstOne)
                {
                    SwitchCompanyWindow.LocalStatus.Value += str;
                }
                else
                {
                    SwitchCompanyWindow.LocalStatus.Value += "," +  str;
                }
                
                firstOne = false;
            }
        }
    }
}
