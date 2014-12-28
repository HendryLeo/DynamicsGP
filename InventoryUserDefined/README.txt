This Project Add User Defined Tables and Window for Inventory Transaction
The Tables are inspired by POP User Defined Tables that allow 5 List, 10 Text and 20 Dates

I will evolve this Project in several phases
1. Get a working Button in Inventory Bin Transfer that will call the User Defined Window (WinForm). All other setup for User Defined Labels and List will be done by SQL means. (this is what I need right now)
2. Get the setup Window (WinForm) working
3. Expand the button to various Inventory related Window

I created a new dexterity dictionary for 3 tables
1. InventoryUDef
2. InventoryUDefSetup
3. InventoryUDefListSetup

The chunk file is included in this repo. If bandwidth allow, I will also upload the modified Dynamics.dic

Create  the assembly by
dag 22888 <your DYNAMICS.SET> /M

InventoryUDef will record UserDefined values for transaction in IV30200 (Inventory Transaction History) and IV30004 (BinToBin Transfer History)

IVDocType used for InventoryUDef are
1 = Adjustment (IN/OUT/BinTransfer)
2 = Variance
3 = Transfer

from http://victoriayudin.com/gp-tables/inventory-tables/
Inventory Transaction Document Types:
1 – Inventory Adjustment
2 – Variance
3 – Inventory Transfer
4 – Purchase Receipt
5 – Sales Returns
6 – Sales Invoices
7 – Assembly
8 – Inventory cost adjustment from POP Shipment (transaction source GLTRX)
11 – Inventory cost adjustment from POP Invoice (transaction source POIVC)


 
