using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
using Microsoft.Dexterity.Applications.VvfIdInHouseCustomizationDictionary;
using Microsoft.Dexterity.Applications.DynamicsDictionary;

namespace InventoryRules
{
    static class DataAccessHelper
    {
        static IvRulesTable IVRulesTable = VvfIdInHouseCustomization.Tables.IvRules;
        static IvRuleTargetsTable IVRuleTargetsTable = VvfIdInHouseCustomization.Tables.IvRuleTargets;
        //static PopReceiptTable POPReceiptTable = Microsoft.Dexterity.Applications.Dynamics.Tables.PopReceipt;
        static PopReceiptUserDefinedTable POPUserDefinedTable = Microsoft.Dexterity.Applications.Dynamics.Tables.PopReceiptUserDefined;


        const byte ROW_FOUND = 0;
        const byte ROW_NOT_FOUND = 1;
        const byte TABLE_ERROR = 2;

        static public TableError GetPOPUserDefinedValues(string POPReceipt, out string[] strs, out DateTime[] date)
        {
            strs = new string[15] { "", "", "", "", "", "", "", "", "", "", "", "", "", "", "" };
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

        static public string GetIVRuleNameByID(short idx)
        {
            return "";
        }

        static public short SetIVRuleNameByID(short idx, string ruleName)
        {

            return 0;
        }

        static public TableError GetIVRuleTargetsByID(short idx, out string[] users)
        {
            TableError lastError, err;
            List<string> list;

            list = new List<string>();

            IVRuleTargetsTable.Key = 1;
            IVRuleTargetsTable.RangeClear();            
            IVRuleTargetsTable.Clear();
            IVRuleTargetsTable.Index.Value = idx;
            IVRuleTargetsTable.RangeStart();
            IVRuleTargetsTable.RangeEnd();

            lastError = IVRuleTargetsTable.GetFirst();
            if (lastError == TableError.NoError)
            {
                err = lastError;
                while (err == TableError.NoError)
                {
                    list.Add(IVRuleTargetsTable.UserId);
                    err = IVRuleTargetsTable.GetNext();
                }
            }

            users = list.ToArray();

            IVRuleTargetsTable.Close();
            return lastError;
        }

        static public short SetIVRuleTargetsByID(short idx, string[] targets)
        {
            return 0;
        }

        static public short DeleteIVRuleByID(short idx)
        {
            return 0;
        }

        static public short DeleteIVRuleTargetsByID(short idx)
        {
            return 0;
        }

        static public short DeleteIVRuleTargetByIDAndUser(short idx, string UserID)
        { 
            return 0;
        }

        static public short DeleteIVRuleTargetsByUser(string UserID)
        {
            return 0;
        }
    }
}
