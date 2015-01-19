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
        
        //System.Timers.Timer aTimer = new System.Timers.Timer();//using timer cause memory corruption, can not figure out how
        //string userActivity = "";

        public void Initialize()
        {
            // Register Event to trigger when Switch Company form opens
            SwitchCompanyWindow.OpenAfterOriginal += new System.EventHandler(SwitchCompanyWindow_OpenAfterOriginal);
            //SwitchCompanyWindow.CloseBeforeOriginal += new System.ComponentModel.CancelEventHandler(SwitchCompanyWindow_CloseBeforeOriginal);
        }

        //void SwitchCompanyWindow_CloseBeforeOriginal(object sender, System.ComponentModel.CancelEventArgs e)
        //{
        //    aTimer.Stop();
        //    //aTimer = null;
        //}

        void SwitchCompanyWindow_OpenAfterOriginal(object sender, EventArgs e)
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

            //create string scrolling procedure here to display all user activity, otherwise Status is limited to 62 char
            foreach (string str in users.ToArray())
            {
                //userActivity += str + ",";
                SwitchCompanyWindow.LocalStatus.Value += str + ",";
            }
            //remove trailing comma
            //userActivity = userActivity.Substring(0, userActivity.Length - 1);
            //if (userActivity.Length > 1)
            //{
            //    aTimer.Elapsed += aTimer_Elapsed;
            //    aTimer.Interval = 1000;
            //    aTimer.Start();
            //}
        }

        //void aTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        //{
        //    //Application.DoEvents();
        //    try
        //    {
        //        if (SwitchCompanyWindow != null)
        //        {
        //            SwitchCompanyWindow.LocalStatus.Value = userActivity;
        //            userActivity = userActivity.Substring(1) + userActivity.Substring(0, 1);
        //        }
        //    }
        //    catch(Exception ex)
        //    {
        //        aTimer.Stop();
                
        //    }
        //}
    }
}
