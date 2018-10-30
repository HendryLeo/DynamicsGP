//credit to
//https://winthropdc.wordpress.com/2014/12/15/customising-the-company-login-window-series-visual-studio-tools-revisited-visual-c/

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
using System.Windows.Forms;
using Microsoft.Dexterity.Applications.DynamicsDictionary;
using Microsoft.Dexterity.Applications.DynamicsModifiedDictionary;

namespace CompanyOptions
{
    using System.ComponentModel;
    //reference to Dynamics Continuum Integration Library breaks VS Tools Application.Dynamics namespace
    //so we use alias for Application.Dynamics
    using GPForms = Microsoft.Dexterity.Applications.Dynamics;
    using GPModifiedForms = Microsoft.Dexterity.Applications.DynamicsModifiedDictionary;

    public class GPAddIn : IDexterityAddIn
    {
        // IDexterityAddIn interface
        static SwitchCompanyForm SwitchCompanyForm = GPForms.Forms.SwitchCompany;
        static SwitchCompanyForm.SwitchCompanyWindow SwitchCompanyWindow = SwitchCompanyForm.SwitchCompany;
        static SyCurrentActivityTable ActivityTable = GPForms.Tables.SyCurrentActivity;

        //Company Color
        static GPModifiedForms.SyCompanyOptionsForm SyCompanyOptionsForm = DynamicsModified.Forms.SyCompanyOptions;
        static GPModifiedForms.SyCompanyOptionsForm.SyCompanyOptionsWindow SyCompanyOptionsWindow = SyCompanyOptionsForm.SyCompanyOptions;
        static Microsoft.Dexterity.Applications.DynamicsDictionary.ToolbarForm ToolbarForm = GPForms.Forms.Toolbar;
        static Microsoft.Dexterity.Applications.DynamicsDictionary.ToolbarForm.MainMenu1Window ToolbarWindow = ToolbarForm.MainMenu1;
        static SyUserDisplayPreferencesForm SyUserDisplayPreferencesForm = GPForms.Forms.SyUserDisplayPreferences;
        static SyUserDisplayPreferencesForm.UserDisplayPreferencesWindow UserDisplayPreferencesWindow = SyUserDisplayPreferencesForm.UserDisplayPreferences;


        const String DUOS_ObjTypeEnv = "Company Colours";


        public void Initialize()
        {
            // Register Event to trigger when Switch Company form opens
            SwitchCompanyWindow.OpenBeforeOriginal += new System.ComponentModel.CancelEventHandler(SwitchCompanyWindow_OpenBeforeOriginal);
            SyCompanyOptionsWindow.LocalSelectApColour.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(SelectAPColour_ClickBeforeOriginal);
            SyCompanyOptionsWindow.LocalSelectTbColour.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(SelectTBColour_ClickBeforeOriginal);
            SyCompanyOptionsWindow.LocalSelectWbColour.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(SelectWBColour_ClickBeforeOriginal);
            SyCompanyOptionsWindow.LocalResetAbColour.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(ResetAPColour_ClickBeforeOriginal);
            SyCompanyOptionsWindow.LocalResetTbColour.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(ResetTBColour_ClickBeforeOriginal);
            SyCompanyOptionsWindow.LocalResetWbColour.ClickBeforeOriginal += new System.ComponentModel.CancelEventHandler(ResetWBColour_ClickBeforeOriginal);
            UserDisplayPreferencesWindow.CloseAfterOriginal += new System.EventHandler(UserDisplayPreferencesWindow_CloseAfterOriginal);
            ToolbarWindow.CompanyName.Change += new System.EventHandler(ToolbarWindow_Change);
        }

        void ToolbarWindow_Change(object sender, EventArgs e)
        {
            ApplyColours();
        }

        void UserDisplayPreferencesWindow_CloseAfterOriginal(object sender, EventArgs e)
        {
            ApplyColours();
        }

        void ResetWBColour_ClickBeforeOriginal(object sender, CancelEventArgs e)
        {
            long Colour = -1;
            string ObjID = "Window Background";
            if (DUOSHelper.SaveDUOSValue(DUOS_ObjTypeEnv, ObjID, "Value", Colour.ToString()))
            {
                //Colour = GetColour(Colour, ObjID);
                SetColour(Colour, ObjID);
            }
        }

        void ResetTBColour_ClickBeforeOriginal(object sender, CancelEventArgs e)
        {
            long Colour = -1;
            string ObjID = "Window Toolbar";
            if (DUOSHelper.SaveDUOSValue(DUOS_ObjTypeEnv, ObjID, "Value", Colour.ToString()))
            {
                //Colour = GetColour(Colour, ObjID);
                SetColour(Colour, ObjID);
            }
        }

        void ResetAPColour_ClickBeforeOriginal(object sender, CancelEventArgs e)
        {
            long Colour = -1;
            string ObjID = "Application Background";
            if(DUOSHelper.SaveDUOSValue(DUOS_ObjTypeEnv, ObjID, "Value", Colour.ToString()))
            {
                //Colour = GetColour(Colour, ObjID);
                SetColour(Colour, ObjID);
            }
        }

        void SelectWBColour_ClickBeforeOriginal(object sender, CancelEventArgs e)
        {
            long Colour;
            string ObjID = "Window Background";
            Colour = Convert.ToInt64(DUOSHelper.GetDUOSValue(DUOS_ObjTypeEnv, ObjID, "Value"));
            Colour = GetColour(Colour, ObjID);
            SetColour(Colour, ObjID);
        }

        void SelectTBColour_ClickBeforeOriginal(object sender, CancelEventArgs e)
        {
            long Colour;
            string ObjID = "Window Toolbar";
            Colour = Convert.ToInt64(DUOSHelper.GetDUOSValue(DUOS_ObjTypeEnv, ObjID, "Value"));
            Colour = GetColour(Colour, ObjID);
            SetColour(Colour, ObjID);
        }

        void SelectAPColour_ClickBeforeOriginal(object sender, CancelEventArgs e)
        {
            long Colour;
            string ObjID = "Application Background";
            Colour = Convert.ToInt64(DUOSHelper.GetDUOSValue(DUOS_ObjTypeEnv, ObjID, "Value"));
            Colour = GetColour(Colour, ObjID);
            SetColour(Colour, ObjID);
        }

        long GetColour(long Colour, string Background)
        {
            string Commands;
            Dynamics.Application CompilerApp = new Dynamics.Application();

            Commands = "";
            Commands = Commands + "local long l_colour; " + System.Environment.NewLine;
            Commands = Commands + "l_colour = " + Colour.ToString() + "; " + System.Environment.NewLine;
            Commands = Commands + "if Color_GetFromDialog(l_colour, COLOR_MODE_SOLID) then " + System.Environment.NewLine;
            Commands = Commands + "  clear table SY_User_Object_Store; " + System.Environment.NewLine;
            Commands = Commands + "  'ObjectType' of table SY_User_Object_Store = \"" + DUOS_ObjTypeEnv + "\"; " + System.Environment.NewLine;
            Commands = Commands + "  'ObjectID' of table SY_User_Object_Store = \"" + Background + "\"; " + System.Environment.NewLine;
            Commands = Commands + "  'PropertyName' of table SY_User_Object_Store = \"" + "Value" + "\"; " + System.Environment.NewLine;
            Commands = Commands + "  change table SY_User_Object_Store; " + System.Environment.NewLine;
            Commands = Commands + "  'PropertyValue' of table SY_User_Object_Store = str(l_colour); " + System.Environment.NewLine;
            Commands = Commands + "  save table SY_User_Object_Store; " + System.Environment.NewLine;
            Commands = Commands + "  check error; " + System.Environment.NewLine;
            Commands = Commands + "end if; " + System.Environment.NewLine;

            string CompilerMessage = "";
            int CompilerError = 0;
            // Execute SanScript in Dynamics GP
            CompilerError = CompilerApp.ExecuteSanscript(Commands, out CompilerMessage);
            if (CompilerError != 0)
            {
                MessageBox.Show(CompilerMessage);
                return -1;
            }
            else
            {
                //DUOS operation
                string value;

                // Retrieve the value
                value = DUOSHelper.GetDUOSValue(DUOS_ObjTypeEnv, Background, "Value");

                return Convert.ToInt64(value);
            }
        }

        void SetColour (long Colour, string Background)
        {
            string Commands;
            Dynamics.Application CompilerApp = new Dynamics.Application();

            Commands = "";
            switch (Background)
            {
                case "Application Background":
                    if(Colour == -1)
                    {
                        Commands = Commands + "Color_ResetSystemColor(SYSTEM_APPLICATION_WORKSPACE); " + System.Environment.NewLine;
                    }
                    else
                    {
                        Commands = Commands + "Color_SetSystemColor(SYSTEM_APPLICATION_WORKSPACE, " + Colour.ToString() + "); " + System.Environment.NewLine;
                    }

                    break;
                case "Window Background":
                    if (Colour == -1)
                    {
                        Commands = Commands + "Color_ResetSystemColor(SYSTEM_BUTTON_FACE); " + System.Environment.NewLine;
                    }
                    else
                    {
                        Commands = Commands + "Color_SetSystemColor(SYSTEM_BUTTON_FACE, " + Colour.ToString() + "); " + System.Environment.NewLine;
                    }

                    break;
                case "Window Toolbar":
                    if (Colour == -1)
                    {
                        Commands = Commands + "Color_ResetSystemColor(SYSTEM_TOOLBAR); " + System.Environment.NewLine;
                        Commands = Commands + "Color_ResetSystemColor(SYSTEM_LIST_HEADER1); " + System.Environment.NewLine;
                        Commands = Commands + "Color_ResetSystemColor(SYSTEM_LIST_HEADER2); " + System.Environment.NewLine;
                    }
                    else
                    {
                        Commands = Commands + "Color_SetSystemColor(SYSTEM_TOOLBAR, " + Colour.ToString() + "); " + System.Environment.NewLine;
                        Commands = Commands + "Color_SetSystemColor(SYSTEM_LIST_HEADER1, " + Colour.ToString() + "); " + System.Environment.NewLine;
                        Commands = Commands + "Color_SetSystemColor(SYSTEM_LIST_HEADER2, " + Colour.ToString() + "); " + System.Environment.NewLine;
                    }

                    break;
                default:
                    break;
            }

            string CompilerMessage = "";
            int CompilerError = 0;
            // Execute SanScript in Dynamics GP
            CompilerError = CompilerApp.ExecuteSanscript(Commands, out CompilerMessage);
            if (CompilerError != 0)
            {
                MessageBox.Show(CompilerMessage);
            }
        }

        void ApplyColours()
        {
            long Colour;
            string ObjID = "Application Background";
            Colour = Convert.ToInt64(DUOSHelper.GetDUOSValue(DUOS_ObjTypeEnv, ObjID, "Value"));
            if (Colour > 0)
            {
                SetColour(Colour, ObjID);
            }
            ObjID = "Window Background";
            Colour = Convert.ToInt64(DUOSHelper.GetDUOSValue(DUOS_ObjTypeEnv, ObjID, "Value"));
            if (Colour > 0)
            {
                SetColour(Colour, ObjID);
            }
            ObjID = "Window Toolbar";
            Colour = Convert.ToInt64(DUOSHelper.GetDUOSValue(DUOS_ObjTypeEnv, ObjID, "Value"));
            if (Colour > 0)
            {
                SetColour(Colour, ObjID);
            }
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
