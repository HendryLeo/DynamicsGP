/* The MIT License (MIT)
 * 
 * Copyright (c) 2014 HendryLeo
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:

 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.

 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
 
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dexterity;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
using Microsoft.Dexterity.Applications.DynamicsDictionary;
using Microsoft.Dexterity.Applications.InventoryUserDefinedDictionary;

namespace InventoryUserDefined
{

    class DataAccessHelper
    {
        static InventoryUserDefinedTable IVUDefTable = Microsoft.Dexterity.Applications.InventoryUserDefined.Tables.InventoryUserDefined ;
        static InventoryUserDefinedSetupTable IVUDefSetupTable = Microsoft.Dexterity.Applications.InventoryUserDefined.Tables.InventoryUserDefinedSetup;
        static InventoryUserDefinedListSetupTable IVUDefListSetupTable = Microsoft.Dexterity.Applications.InventoryUserDefined.Tables.InventoryUserDefinedListSetup ;
        const byte ROW_FOUND = 0;
        const byte ROW_NOT_FOUND = 1;
        const byte TABLE_ERROR = 2;

        static public TableError GetIVUDefListSetup(byte idx, out string[] strs)
        {
            TableError lastError, err;
            List<string> list = new List<string>();

            IVUDefListSetupTable.Key = 1;
            IVUDefListSetupTable.Clear();
            IVUDefListSetupTable.Index.Value = idx++;
            IVUDefListSetupTable.RangeStart();

            IVUDefListSetupTable.Clear();
            IVUDefListSetupTable.Index.Value = idx; 
            IVUDefListSetupTable.RangeEnd();

            lastError = IVUDefListSetupTable.GetFirst();

            if (lastError == TableError.NoError)
            {
                err = lastError;
                while (err == TableError.NoError)
                {
                    list.Add(IVUDefListSetupTable.Description);
                    err = IVUDefListSetupTable.GetNext();
                }
            }

            strs = list.ToArray();

            IVUDefListSetupTable.Close();
            return lastError;
        }

        static public TableError DeleteIVUDefValues(string IVTrx, string IVTrxType)
        {
            TableError lastError;
            byte checkExist;

            checkExist = UserDefinedExist(IVTrx, IVTrxType, out lastError);
            switch (checkExist)
            {
                case TABLE_ERROR:
                    return lastError;

                case ROW_FOUND:
                    IVUDefTable.Key = 1;
                    IVUDefTable.DocumentNumber.Value = IVTrx;
                    IVUDefTable.DocumentType.Value = Convert.ToInt16(IVTrxType);
                    lastError = IVUDefTable.Change();
                    break;

                case ROW_NOT_FOUND:
                    return lastError;

            }

            lastError = IVUDefTable.Remove();
            IVUDefTable.Close();
            return lastError;
        }

        static public TableError SetIVUDefValues(string IVTrx, string IVTrxType, string[] strs, DateTime[] date)
        {

            TableError lastError;
            byte checkExist;

            checkExist = UserDefinedExist(IVTrx, IVTrxType, out lastError);

            switch(checkExist)
            {
                case TABLE_ERROR:
                    return lastError;
                
                case ROW_FOUND:
                    IVUDefTable.Key = 1;
                    IVUDefTable.DocumentNumber.Value = IVTrx;
                    IVUDefTable.DocumentType.Value = Convert.ToInt16(IVTrxType);
                    lastError = IVUDefTable.Change();
                    break;
  
                case ROW_NOT_FOUND:
                    lastError = IVUDefTable.Clear();
                    break;

            }
            if (lastError != TableError.NoError)
            {
                //error retrieving/locking/clearing record
                return lastError;
            }

            IVUDefTable.DocumentNumber.Value = IVTrx;
            IVUDefTable.DocumentType.Value = Convert.ToInt16(IVTrxType);
            IVUDefTable.UserDefinedList01.Value = strs[0];
            IVUDefTable.UserDefinedList02.Value = strs[1];
            IVUDefTable.UserDefinedList03.Value = strs[2];
            IVUDefTable.UserDefinedList04.Value = strs[3];
            IVUDefTable.UserDefinedList05.Value = strs[4];
            IVUDefTable.UserDefinedText01.Value = strs[5];
            IVUDefTable.UserDefinedText02.Value = strs[6];
            IVUDefTable.UserDefinedText03.Value = strs[7];
            IVUDefTable.UserDefinedText04.Value = strs[8];
            IVUDefTable.UserDefinedText05.Value = strs[9];
            IVUDefTable.UserDefinedText06.Value = strs[10];
            IVUDefTable.UserDefinedText07.Value = strs[11];
            IVUDefTable.UserDefinedText08.Value = strs[12];
            IVUDefTable.UserDefinedText09.Value = strs[13];
            IVUDefTable.UserDefinedText10.Value = strs[14];

            IVUDefTable.UserDefinedDate01.Value = date[0];
            IVUDefTable.UserDefinedDate02.Value = date[1];
            IVUDefTable.UserDefinedDate03.Value = date[2];
            IVUDefTable.UserDefinedDate04.Value = date[3];
            IVUDefTable.UserDefinedDate05.Value = date[4];
            IVUDefTable.UserDefinedDate06.Value = date[5];
            IVUDefTable.UserDefinedDate07.Value = date[6];
            IVUDefTable.UserDefinedDate08.Value = date[7];
            IVUDefTable.UserDefinedDate09.Value = date[8];
            IVUDefTable.UserDefinedDate10.Value = date[9];
            IVUDefTable.UserDefinedDate11.Value = date[10];
            IVUDefTable.UserDefinedDate12.Value = date[11];
            IVUDefTable.UserDefinedDate13.Value = date[12];
            IVUDefTable.UserDefinedDate14.Value = date[13];
            IVUDefTable.UserDefinedDate15.Value = date[14];
            IVUDefTable.UserDefinedDate16.Value = date[15];
            IVUDefTable.UserDefinedDate17.Value = date[16];
            IVUDefTable.UserDefinedDate18.Value = date[17];
            IVUDefTable.UserDefinedDate19.Value = date[18];
            IVUDefTable.UserDefinedDate20.Value = date[19];
            
            lastError = IVUDefTable.Save();

            IVUDefTable.Close();
            return lastError;
        }

        static public TableError GetIVUDefValues(string IVTrx, string IVTrxType, out string[] strs, out DateTime[] date)
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
            IVUDefTable.Key = 1;
            IVUDefTable.DocumentNumber.Value = IVTrx;
            IVUDefTable.DocumentType.Value = Convert.ToInt16(IVTrxType);
            lastError = IVUDefTable.Get();

            if (lastError == TableError.NoError) //row found
            {
                strs[0] = IVUDefTable.UserDefinedList01.Value;
                strs[1] = IVUDefTable.UserDefinedList02.Value;
                strs[2] = IVUDefTable.UserDefinedList03.Value;
                strs[3] = IVUDefTable.UserDefinedList04.Value;
                strs[4] = IVUDefTable.UserDefinedList05.Value;
                strs[5] = IVUDefTable.UserDefinedText01.Value;
                strs[6] = IVUDefTable.UserDefinedText02.Value;
                strs[7] = IVUDefTable.UserDefinedText03.Value;
                strs[8] = IVUDefTable.UserDefinedText04.Value;
                strs[9] = IVUDefTable.UserDefinedText05.Value;
                strs[10] = IVUDefTable.UserDefinedText06.Value;
                strs[11] = IVUDefTable.UserDefinedText07.Value;
                strs[12] = IVUDefTable.UserDefinedText08.Value;
                strs[13] = IVUDefTable.UserDefinedText09.Value;
                strs[14] = IVUDefTable.UserDefinedText10.Value;

                date[0] = IVUDefTable.UserDefinedDate01.Value;
                date[1] = IVUDefTable.UserDefinedDate02.Value;
                date[2] = IVUDefTable.UserDefinedDate03.Value;
                date[3] = IVUDefTable.UserDefinedDate04.Value;
                date[4] = IVUDefTable.UserDefinedDate05.Value;
                date[5] = IVUDefTable.UserDefinedDate06.Value;
                date[6] = IVUDefTable.UserDefinedDate07.Value;
                date[7] = IVUDefTable.UserDefinedDate08.Value;
                date[8] = IVUDefTable.UserDefinedDate09.Value;
                date[9] = IVUDefTable.UserDefinedDate10.Value;
                date[10] = IVUDefTable.UserDefinedDate11.Value;
                date[11] = IVUDefTable.UserDefinedDate12.Value;
                date[12] = IVUDefTable.UserDefinedDate13.Value;
                date[13] = IVUDefTable.UserDefinedDate14.Value;
                date[14] = IVUDefTable.UserDefinedDate15.Value;
                date[15] = IVUDefTable.UserDefinedDate16.Value;
                date[16] = IVUDefTable.UserDefinedDate17.Value;
                date[17] = IVUDefTable.UserDefinedDate18.Value;
                date[18] = IVUDefTable.UserDefinedDate19.Value;
                date[19] = IVUDefTable.UserDefinedDate20.Value;
                

            }

            IVUDefTable.Close();
            return lastError;
        }

        static public TableError GetIVUDefSetup(out string[] strs)
        {
            strs = new string[35] 
                {"", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
                 "", "", "", "", "", "", "", "", "", "", "", "", "", "", "", 
                 "", "", "", "", ""};

            TableError lastError;

            IVUDefSetupTable.Key = 1;
            IVUDefSetupTable.Index.Value = 1;
            lastError = IVUDefSetupTable.Get();

            if (lastError == TableError.NoError)
            {
                //There must be better way to do this in loop
                strs[0] = IVUDefSetupTable.UserDefinedPrompt01.Value;
                strs[1] = IVUDefSetupTable.UserDefinedPrompt02.Value;
                strs[2] = IVUDefSetupTable.UserDefinedPrompt03.Value;
                strs[3] = IVUDefSetupTable.UserDefinedPrompt04.Value;
                strs[4] = IVUDefSetupTable.UserDefinedPrompt05.Value;
                strs[5] = IVUDefSetupTable.UserDefinedPrompt06.Value;
                strs[6] = IVUDefSetupTable.UserDefinedPrompt07.Value;
                strs[7] = IVUDefSetupTable.UserDefinedPrompt08.Value;
                strs[8] = IVUDefSetupTable.UserDefinedPrompt09.Value;
                strs[9] = IVUDefSetupTable.UserDefinedPrompt10.Value;
                strs[10] = IVUDefSetupTable.UserDefinedPrompt11.Value;
                strs[11] = IVUDefSetupTable.UserDefinedPrompt12.Value;
                strs[12] = IVUDefSetupTable.UserDefinedPrompt13.Value;
                strs[13] = IVUDefSetupTable.UserDefinedPrompt14.Value;
                strs[14] = IVUDefSetupTable.UserDefinedPrompt15.Value;
                strs[15] = IVUDefSetupTable.UserDefinedPrompt16.Value;
                strs[16] = IVUDefSetupTable.UserDefinedPrompt17.Value;
                strs[17] = IVUDefSetupTable.UserDefinedPrompt18.Value;
                strs[18] = IVUDefSetupTable.UserDefinedPrompt19.Value;
                strs[19] = IVUDefSetupTable.UserDefinedPrompt20.Value;
                strs[20] = IVUDefSetupTable.UserDefinedPrompt21.Value;
                strs[21] = IVUDefSetupTable.UserDefinedPrompt22.Value;
                strs[22] = IVUDefSetupTable.UserDefinedPrompt23.Value;
                strs[23] = IVUDefSetupTable.UserDefinedPrompt24.Value;
                strs[24] = IVUDefSetupTable.UserDefinedPrompt25.Value;
                strs[25] = IVUDefSetupTable.UserDefinedPrompt26.Value;
                strs[26] = IVUDefSetupTable.UserDefinedPrompt27.Value;
                strs[27] = IVUDefSetupTable.UserDefinedPrompt28.Value;
                strs[28] = IVUDefSetupTable.UserDefinedPrompt29.Value;
                strs[29] = IVUDefSetupTable.UserDefinedPrompt30.Value;
                strs[30] = IVUDefSetupTable.UserDefinedPrompt31.Value;
                strs[31] = IVUDefSetupTable.UserDefinedPrompt32.Value;
                strs[32] = IVUDefSetupTable.UserDefinedPrompt33.Value;
                strs[33] = IVUDefSetupTable.UserDefinedPrompt34.Value;
                strs[34] = IVUDefSetupTable.UserDefinedPrompt35.Value;
            }

            IVUDefSetupTable.Close();
            return lastError;
        }

        static byte UserDefinedExist(string IVTrx, string IVTrxType, out TableError lastError)
        {
            //TableError lastError;
            IVUDefTable.Key = 1;
            IVUDefTable.DocumentNumber.Value = IVTrx;
            IVUDefTable.DocumentType.Value = Convert.ToInt16(IVTrxType);
            lastError = IVUDefTable.Get();

            if (lastError != TableError.NoError)
            {
                if (lastError != TableError.NotFound) //error other than NotFound
                {
                    //Microsoft.Dexterity.Applications.Dynamics.Forms.SyVisualStudioHelper.Functions.DexError.Invoke(lastError.ToString());
                    IVUDefTable.Close();
                    return TABLE_ERROR; 
                }
                else //NotFound
                {
                    IVUDefTable.Close();
                    return ROW_NOT_FOUND;
                }
            }

            IVUDefTable.Close();
            return ROW_FOUND;
        }
    }
}
