This project use additional dictionary called VVFHOUSE.DIC that can be found in folder DexterityDictionary

This Project is to enforce business rules that can not be achieved using GP settings alone
opt-in rule, is rule that user subscribe by request
opt-out rule, is enforced by default, user can request to be exempted

1 (opt-out). Inventory Transaction Entry - can not post from transaction entry (must use batch post)
2 (opt-in). Inventory Transaction Entry - user have default site = "STORE", batch = "ISSUE SEMENTARA" and adjustment type = 2, if doing adjustment type = 1, will be prompted
3 (opt-out). Shipment (Receiving Trx Entry) must use server Date and can not edit Receipt that has certain userdefined value
4 (opt-in). Item Transfer Entry - user can only transfer from site "PROD" (any bin) to "LOGISTIC" (bin "WB STORE")
5 (opt-out). Void Open Payable Transaction, can not void invoices that originated from POP
6 (opt-in). user can edit User Defined after post
7 (opt-out). Item Maintenance - item quantity decimal must not be less than 2, Item valuation method is average perpetual
8 (opt-out). Matching Invoice date can not be less than Receipt Date
9 (opt-in). user can only edit Receipt Date and save to batch

As usual, I have several alternate form
rule no 2, 4 is tightly coupled with my GP setup

This is the sanscript for Rule 3, to open POPInquiryReceivingEntry
'OpenWindow() of form POP_Inquiry_Receivings_Entry', 0, "GRN1400953", 2, 3, 2 ==> Posted Receipt Open PO
'OpenWindow() of form POP_Inquiry_Receivings_Entry', 0, "GRN1401157", 2, 4, 2 ==> Saved Receipt Open PO
'OpenWindow() of form POP_Inquiry_Receivings_Entry', 0, "GRN1400356", 2, 3, 2 ==> Posted Receipt History PO

to use rule no 3, you will need a login to SQLServer with name GPDateGetter, with random password that you will need to change from source code