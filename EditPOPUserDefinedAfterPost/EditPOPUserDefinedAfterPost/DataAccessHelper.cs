using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dexterity;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
using Microsoft.Dexterity.Applications.DynamicsDictionary;

namespace EditPOPUserDefinedAfterPost
{

    class DataAccessHelper
    {
        static PopReceiptUserDefinedTable POPUserDefinedTable = Microsoft.Dexterity.Applications.Dynamics.Tables.PopReceiptUserDefined;
        static PopSetupTable POPSetupTable = Microsoft.Dexterity.Applications.Dynamics.Tables.PopSetup;
        static PopUserDefinedSetupTable POPUserDefinedSetupTable = Microsoft.Dexterity.Applications.Dynamics.Tables.PopUserDefinedSetup;
        const byte ROW_FOUND = 0;
        const byte ROW_NOT_FOUND = 1;
        const byte TABLE_ERROR = 2;

        static public TableError GetPOPUserDefinedSetup(byte idx, out string[] strs)
        {
            TableError lastError, err;
            List<string> list = new List<string>();

            POPUserDefinedSetupTable.Key = 1;
            POPUserDefinedSetupTable.Clear();
            POPUserDefinedSetupTable.Index.Value = idx++;
            POPUserDefinedSetupTable.RangeStart();

            POPUserDefinedSetupTable.Clear();
            POPUserDefinedSetupTable.Index.Value = idx; 
            POPUserDefinedSetupTable.RangeEnd();

            lastError = POPUserDefinedSetupTable.GetFirst();

            if (lastError == TableError.NoError)
            {
                err = lastError;
                while (err == TableError.NoError)
                {
                    list.Add(POPUserDefinedSetupTable.Description);
                    err = POPUserDefinedSetupTable.GetNext();
                }
            }

            strs = list.ToArray();

            POPUserDefinedSetupTable.Close();
            return lastError;
        }

        static public TableError DeletePOPUsrDefinedValues(string POPReceipt)
        {
            TableError lastError;
            byte checkExist;

            checkExist = UserDefinedExist(POPReceipt, out lastError);
            switch (checkExist)
            {
                case TABLE_ERROR:
                    return lastError;

                case ROW_FOUND:
                    POPUserDefinedTable.Key = 1;
                    POPUserDefinedTable.PopReceiptNumber.Value = POPReceipt;
                    lastError = POPUserDefinedTable.Change();
                    break;

                case ROW_NOT_FOUND:
                    return lastError;

            }

            lastError = POPUserDefinedTable.Remove();

            return lastError;
        }

        static public TableError SetPOPUserDefinedValues(string POPReceipt, string[] strs, DateTime[] date)
        {

            TableError lastError;
            byte checkExist;

            checkExist = UserDefinedExist(POPReceipt, out lastError);

            switch(checkExist)
            {
                case TABLE_ERROR:
                    return lastError;
                
                case ROW_FOUND:
                    POPUserDefinedTable.Key = 1;
                    POPUserDefinedTable.PopReceiptNumber.Value = POPReceipt;
                    lastError = POPUserDefinedTable.Change();
                    break;
  
                case ROW_NOT_FOUND:
                    lastError = POPUserDefinedTable.Clear();
                    break;

            }
            if (lastError != TableError.NoError)
            {
                //error retrieving/locking/clearing record
                return lastError;
            }

            POPUserDefinedTable.PopReceiptNumber.Value = POPReceipt;
            POPUserDefinedTable.UserDefinedList01.Value = strs[0];
            POPUserDefinedTable.UserDefinedList02.Value = strs[1];
            POPUserDefinedTable.UserDefinedList03.Value = strs[2];
            POPUserDefinedTable.UserDefinedList04.Value = strs[3];
            POPUserDefinedTable.UserDefinedList05.Value = strs[4];
            POPUserDefinedTable.UserDefinedText01.Value = strs[5];
            POPUserDefinedTable.UserDefinedText02.Value = strs[6];
            POPUserDefinedTable.UserDefinedText03.Value = strs[7];
            POPUserDefinedTable.UserDefinedText04.Value = strs[8];
            POPUserDefinedTable.UserDefinedText05.Value = strs[9];
            POPUserDefinedTable.UserDefinedText06.Value = strs[10];
            POPUserDefinedTable.UserDefinedText07.Value = strs[11];
            POPUserDefinedTable.UserDefinedText08.Value = strs[12];
            POPUserDefinedTable.UserDefinedText09.Value = strs[13];
            POPUserDefinedTable.UserDefinedText10.Value = strs[14];

            POPUserDefinedTable.UserDefinedDate01.Value = date[0];
            POPUserDefinedTable.UserDefinedDate02.Value = date[1];
            POPUserDefinedTable.UserDefinedDate03.Value = date[2];
            POPUserDefinedTable.UserDefinedDate04.Value = date[3];
            POPUserDefinedTable.UserDefinedDate05.Value = date[4];
            POPUserDefinedTable.UserDefinedDate06.Value = date[5];
            POPUserDefinedTable.UserDefinedDate07.Value = date[6];
            POPUserDefinedTable.UserDefinedDate08.Value = date[7];
            POPUserDefinedTable.UserDefinedDate09.Value = date[8];
            POPUserDefinedTable.UserDefinedDate10.Value = date[9];
            POPUserDefinedTable.UserDefinedDate11.Value = date[10];
            POPUserDefinedTable.UserDefinedDate12.Value = date[11];
            POPUserDefinedTable.UserDefinedDate13.Value = date[12];
            POPUserDefinedTable.UserDefinedDate14.Value = date[13];
            POPUserDefinedTable.UserDefinedDate15.Value = date[14];
            POPUserDefinedTable.UserDefinedDate16.Value = date[15];
            POPUserDefinedTable.UserDefinedDate17.Value = date[16];
            POPUserDefinedTable.UserDefinedDate18.Value = date[17];
            POPUserDefinedTable.UserDefinedDate19.Value = date[18];
            POPUserDefinedTable.UserDefinedDate20.Value = date[19];
            
            lastError = POPUserDefinedTable.Save();

            POPUserDefinedTable.Close();
            return lastError;
        }

        static public TableError GetPOPUserDefinedValues(string POPReceipt, out string[] strs, out DateTime[] date)
        {
            strs = new string[15] 
                {"", "", "", "", "", "", "", "", "", "", "", "", "", "", ""};
            date = new DateTime[20]
                {new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1),
                 new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1),
                 new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1),
                 new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1),
                 new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1), new DateTime(1900,1,1)};
            
            TableError lastError;
            POPUserDefinedTable.Key = 1;
            POPUserDefinedTable.PopReceiptNumber.Value = POPReceipt;
            lastError = POPUserDefinedTable.Get();

            if (lastError == TableError.NoError) //row found
            {
                strs[0] = POPUserDefinedTable.UserDefinedList01.Value;
                strs[1] = POPUserDefinedTable.UserDefinedList02.Value;
                strs[2] = POPUserDefinedTable.UserDefinedList03.Value;
                strs[3] = POPUserDefinedTable.UserDefinedList04.Value;
                strs[4] = POPUserDefinedTable.UserDefinedList05.Value;
                strs[5] = POPUserDefinedTable.UserDefinedText01.Value;
                strs[6] = POPUserDefinedTable.UserDefinedText02.Value;
                strs[7] = POPUserDefinedTable.UserDefinedText03.Value;
                strs[8] = POPUserDefinedTable.UserDefinedText04.Value;
                strs[9] = POPUserDefinedTable.UserDefinedText05.Value;
                strs[10] = POPUserDefinedTable.UserDefinedText06.Value;
                strs[11] = POPUserDefinedTable.UserDefinedText07.Value;
                strs[12] = POPUserDefinedTable.UserDefinedText08.Value;
                strs[13] = POPUserDefinedTable.UserDefinedText09.Value;
                strs[14] = POPUserDefinedTable.UserDefinedText10.Value;

                date[0] = POPUserDefinedTable.UserDefinedDate01.Value;
                date[1] = POPUserDefinedTable.UserDefinedDate02.Value;
                date[2] = POPUserDefinedTable.UserDefinedDate03.Value;
                date[3] = POPUserDefinedTable.UserDefinedDate04.Value;
                date[4] = POPUserDefinedTable.UserDefinedDate05.Value;
                date[5] = POPUserDefinedTable.UserDefinedDate06.Value;
                date[6] = POPUserDefinedTable.UserDefinedDate07.Value;
                date[7] = POPUserDefinedTable.UserDefinedDate08.Value;
                date[8] = POPUserDefinedTable.UserDefinedDate09.Value;
                date[9] = POPUserDefinedTable.UserDefinedDate10.Value;
                date[10] = POPUserDefinedTable.UserDefinedDate11.Value;
                date[11] = POPUserDefinedTable.UserDefinedDate12.Value;
                date[12] = POPUserDefinedTable.UserDefinedDate13.Value;
                date[13] = POPUserDefinedTable.UserDefinedDate14.Value;
                date[14] = POPUserDefinedTable.UserDefinedDate15.Value;
                date[15] = POPUserDefinedTable.UserDefinedDate16.Value;
                date[16] = POPUserDefinedTable.UserDefinedDate17.Value;
                date[17] = POPUserDefinedTable.UserDefinedDate18.Value;
                date[18] = POPUserDefinedTable.UserDefinedDate19.Value;
                date[19] = POPUserDefinedTable.UserDefinedDate20.Value;
                

            }

            POPUserDefinedTable.Close();
            return lastError;
        }

        static public TableError GetPOPSetup(out string[] strs)
        {
            strs = new string[35] 
                {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
                 "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
                 "", "", "", "", ""};

            TableError lastError;

            POPSetupTable.Key = 1;
            POPSetupTable.Index.Value = 1;
            lastError = POPSetupTable.Get();

            if (lastError == TableError.NoError)
            {
                //There must be better way to do this in loop
                strs[0] = POPSetupTable.UserDefinedPrompt01.Value;
                strs[1] = POPSetupTable.UserDefinedPrompt02.Value;
                strs[2] = POPSetupTable.UserDefinedPrompt03.Value;
                strs[3] = POPSetupTable.UserDefinedPrompt04.Value;
                strs[4] = POPSetupTable.UserDefinedPrompt05.Value;
                strs[5] = POPSetupTable.UserDefinedPrompt06.Value;
                strs[6] = POPSetupTable.UserDefinedPrompt07.Value;
                strs[7] = POPSetupTable.UserDefinedPrompt08.Value;
                strs[8] = POPSetupTable.UserDefinedPrompt09.Value;
                strs[9] = POPSetupTable.UserDefinedPrompt10.Value;
                strs[10] = POPSetupTable.UserDefinedPrompt11.Value;
                strs[11] = POPSetupTable.UserDefinedPrompt12.Value;
                strs[12] = POPSetupTable.UserDefinedPrompt13.Value;
                strs[13] = POPSetupTable.UserDefinedPrompt14.Value;
                strs[14] = POPSetupTable.UserDefinedPrompt15.Value;
                strs[15] = POPSetupTable.UserDefinedPrompt16.Value;
                strs[16] = POPSetupTable.UserDefinedPrompt17.Value;
                strs[17] = POPSetupTable.UserDefinedPrompt18.Value;
                strs[18] = POPSetupTable.UserDefinedPrompt19.Value;
                strs[19] = POPSetupTable.UserDefinedPrompt20.Value;
                strs[20] = POPSetupTable.UserDefinedPrompt21.Value;
                strs[21] = POPSetupTable.UserDefinedPrompt22.Value;
                strs[22] = POPSetupTable.UserDefinedPrompt23.Value;
                strs[23] = POPSetupTable.UserDefinedPrompt24.Value;
                strs[24] = POPSetupTable.UserDefinedPrompt25.Value;
                strs[25] = POPSetupTable.UserDefinedPrompt26.Value;
                strs[26] = POPSetupTable.UserDefinedPrompt27.Value;
                strs[27] = POPSetupTable.UserDefinedPrompt28.Value;
                strs[28] = POPSetupTable.UserDefinedPrompt29.Value;
                strs[29] = POPSetupTable.UserDefinedPrompt30.Value;
                strs[30] = POPSetupTable.UserDefinedPrompt31.Value;
                strs[31] = POPSetupTable.UserDefinedPrompt32.Value;
                strs[32] = POPSetupTable.UserDefinedPrompt33.Value;
                strs[33] = POPSetupTable.UserDefinedPrompt34.Value;
                strs[34] = POPSetupTable.UserDefinedPrompt35.Value;
            }

            POPSetupTable.Close();
            return lastError;
        }

        //static public void ClosePOPUserDefinedTable()
        //{
        //    POPUserDefinedTable.Close();
        //}

        //static byte OpenPOPUserDefinedTable(string POPReceipt)
        //{
        //    TableError lastError;
        //    POPUserDefinedTable.Key = 1;
        //    POPUserDefinedTable.PopReceiptNumber.Value = POPReceipt;
        //    lastError = POPUserDefinedTable.Get();

        //    if (lastError != TableError.NoError)
        //    {
        //        if (lastError != TableError.NotFound)
        //        {
        //            Microsoft.Dexterity.Applications.Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(lastError.ToString());
        //            POPUserDefinedTable.Close();
        //            return Constants.TABLE_ERROR;
        //        }
        //        else //NotFound
        //        {
        //            POPUserDefinedTable.Close();
        //            return Constants.ROW_NOT_FOUND;
        //        }
        //    }

        //    POPUserDefinedTable.Close();
        //    return Constants.ROW_FOUND;
        //}

        static byte UserDefinedExist(string POPReceipt, out TableError lastError)
        {
            //TableError lastError;
            POPUserDefinedTable.Key = 1;
            POPUserDefinedTable.PopReceiptNumber.Value = POPReceipt;
            lastError = POPUserDefinedTable.Get();

            if (lastError != TableError.NoError)
            {
                if (lastError != TableError.NotFound) //error other than NotFound
                {
                    Microsoft.Dexterity.Applications.Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(lastError.ToString());
                    POPUserDefinedTable.Close();
                    return TABLE_ERROR; 
                }
                else //NotFound
                {
                    POPUserDefinedTable.Close();
                    return ROW_NOT_FOUND;
                }
            }

            POPUserDefinedTable.Close();
            return ROW_FOUND;
        }
    }
}
