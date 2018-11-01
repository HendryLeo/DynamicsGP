This Project add 2 additional field to Journal Transaction Entry and Journal Entry Inquiry to facilitate additional Vendors Info for GL Entry in table GL10001
2 lookup button to facilitate Vendor Lookup and Customer Lookup

Please import 2 Package to GP Customization Maintainance
GL_Journal_Entry_Inquiry.package
GL_Transaction_Entry.package

I am using modified window, so create assembly with
dag 0 <your DYNAMICS.SET> /f /n:Dynamics
change id and path to DYNAMICS.SET as your environment
Copy the resulting DLL to Addins Folder
