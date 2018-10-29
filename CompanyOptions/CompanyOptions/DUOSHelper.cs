using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Dexterity;
using Microsoft.Dexterity.Bridge;
using Microsoft.Dexterity.Applications;
using Microsoft.Dexterity.Applications.DynamicsDictionary;

namespace CompanyOptions
{
    //reference to Dynamics Continuum Integration Library breaks VS Tools Application.Dynamics namespace
    //so we use alias for Application.Dynamics
    using GPForms = Microsoft.Dexterity.Applications;

    class DUOSHelper
    {
        public static string GetDUOSValue(String ObjectType, String ObjectID, String PropertyName)
        {
            TableError lastError;

            SyUserObjectStoreTable DUOSTable;
            DUOSTable = GPForms.Dynamics.Tables.SyUserObjectStore;

            string value;

            // Get the DUOS property value
            DUOSTable.Key = 1;
            DUOSTable.ObjectType.Value = ObjectType;
            DUOSTable.ObjectId.Value = ObjectID;
            DUOSTable.PropertyName.Value = PropertyName;

            // Attempt to read the row.
            lastError = DUOSTable.Get();

            if (lastError == TableError.NoError)
            {
                value = DUOSTable.PropertyValue.Value;
                DUOSTable.Close();
                return value;
            }
            else
            {
                DUOSTable.Close();
                return "";
            }
        }


        public static Boolean SaveDUOSValue(String ObjectType, String ObjectID, String PropertyName, String PropertyValue)
        {
            TableError lastError;

            SyUserObjectStoreTable DUOSTable;
            DUOSTable = GPForms.Dynamics.Tables.SyUserObjectStore;

            // Save the DUOS property value
            DUOSTable.Key = 1;
            DUOSTable.ObjectType.Value = ObjectType;
            DUOSTable.ObjectId.Value = ObjectID;
            DUOSTable.PropertyName.Value = PropertyName;

            // Attempt to read the row. The Change() method will lock the row.
            lastError = DUOSTable.Change();

            if ((lastError == TableError.NoError) || (lastError == TableError.NotFound))
            {
                // If the row already exists, it will be udpated. If it does not, it will be created.
                DUOSTable.PropertyValue.Value = PropertyValue;
                lastError = DUOSTable.Save();
                if (lastError == TableError.NoError)
                {
                    DUOSTable.Close();
                    return true;
                }
                else
                {
                    DUOSTable.Close();
                    return false;
                }
            }
            else
            {
                // Something went wrong trying to lock an existing row.
                DUOSTable.Close();
                return false;
            }
        }


        public static Boolean DeleteDUOSValue(String ObjectType, String ObjectID, String PropertyName)
        {
            TableError lastError;

            SyUserObjectStoreTable DUOSTable;
            DUOSTable = GPForms.Dynamics.Tables.SyUserObjectStore;

            // Delete the DUOS property value
            DUOSTable.Key = 1;
            DUOSTable.ObjectType.Value = ObjectType;
            DUOSTable.ObjectId.Value = ObjectID;
            DUOSTable.PropertyName.Value = PropertyName;

            // Attempt to read the row. The Change() method will lock the row.
            lastError = DUOSTable.Change();

            if (lastError == TableError.NoError)
            {
                // A row was found, so attempt to delete the property
                lastError = DUOSTable.Remove();

                if (lastError == TableError.NoError)
                {
                    DUOSTable.Close();
                    return true;
                }
                else
                {
                    DUOSTable.Close();
                    return false;
                }
            }
            
            // No row to delete, so return true.
            DUOSTable.Close();
            return true;
        }
    }
}
