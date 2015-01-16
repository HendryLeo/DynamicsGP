using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Dexterity;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
using Microsoft.Dexterity.Applications.DynamicsDictionary;

namespace JournalEntryWithVendorInfo
{
    static class DataAccessHelper
    {
        static GlTrxLineWorkTable GLTrxLineWorkTable = Dynamics.Tables.GlTrxLineWork;
        const byte ROW_FOUND = 0;
        const byte ROW_NOT_FOUND = 1;
        const byte TABLE_ERROR = 2;

        static public TableError GetGLTrxLineByJournalEntry(int jrnEntry, out string id, out string name)
        {
            TableError lastError;
            byte checkExist;
            id = "";
            name = "";
            checkExist = GLTrxLineExistByJournalEntry(jrnEntry, out lastError);
            switch (checkExist)
            {
                case ROW_FOUND:
                    GLTrxLineWorkTable.Key = 4;

                    GLTrxLineWorkTable.RangeClear();
                    GLTrxLineWorkTable.JournalEntry.Value = jrnEntry;

                    GLTrxLineWorkTable.RangeStart();
                    GLTrxLineWorkTable.RangeEnd();

                    lastError = GLTrxLineWorkTable.GetFirst();

                    id = GLTrxLineWorkTable.OriginatingMasterId.Value;
                    name = GLTrxLineWorkTable.OriginatingMasterName.Value;
                    
                    GLTrxLineWorkTable.Close();
                    return lastError;
                default:
                    return lastError;
            }
        }

        static public TableError UpdateGLTrxLineByJournalEntry(int jrnEntry, string id, string name)
        {
            TableError lastError;
            byte checkExist;

            checkExist = GLTrxLineExistByJournalEntry(jrnEntry, out lastError);
            switch (checkExist)
            {
                case ROW_FOUND:
                    GLTrxLineWorkTable.Key = 4;

                    GLTrxLineWorkTable.RangeClear();
                    GLTrxLineWorkTable.JournalEntry.Value = jrnEntry;
                    
                    GLTrxLineWorkTable.RangeStart();
                    GLTrxLineWorkTable.RangeEnd();

                    lastError = GLTrxLineWorkTable.ChangeFirst();
                    
                    while (lastError == TableError.NoError)
                    {
                        GLTrxLineWorkTable.JournalEntry.Value = jrnEntry;
                        GLTrxLineWorkTable.OriginatingMasterId.Value = id;
                        GLTrxLineWorkTable.OriginatingMasterName.Value = name;
                        GLTrxLineWorkTable.Save();
                        lastError = GLTrxLineWorkTable.ChangeNext();
                    }
                    if (lastError == TableError.EndOfTable) lastError = TableError.NoError;
                    GLTrxLineWorkTable.Close();
                    return lastError;
                default:
                    return lastError;
            }
        }

        static byte GLTrxLineExistByJournalEntry(int jrnEntry, out TableError lastError)
        {
            GLTrxLineWorkTable.Key = 4;
            GLTrxLineWorkTable.JournalEntry.Value = jrnEntry;
            GLTrxLineWorkTable.RangeStart();
            GLTrxLineWorkTable.RangeEnd();

            lastError = GLTrxLineWorkTable.GetFirst();

            if (lastError != TableError.NoError)
            {
                if (lastError != TableError.NotFound) //error other than not found
                {
                    GLTrxLineWorkTable.Close();
                    return TABLE_ERROR;
                }
                else //Not Found
                {
                    return ROW_NOT_FOUND;
                }
            }

            GLTrxLineWorkTable.Close();
            return ROW_FOUND;
        }
    }
}
