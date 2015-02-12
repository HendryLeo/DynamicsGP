this project is a hack around limitation of third party customization
we have VAT module done by our consultant that work roughly like this
1. post the VAT info to third party table
2. create Payable Invoice in the currency that is derived from VAT info

as we implement this we realize that our VAT should not be in foreign currency and must be in functional currency
this is the attempt to hack around this VAT module
the idea is
1. hook to posting routine (PRE)
2. if currency id is functional, then resume normal operation (no hack)
3. if currency id is not functional, then create Payable Invoice in functional currency using the same Voucher Number, then resume normal operation
4. normal operation will be interrupted while posting Payable Invoice because Voucher Number already taken in step 3.
5. normal operation created Payable Invoice, need to delete that trx. careful that GL distribution is the same with bullet 3 and can be deleted if using normal delete

create invoice and save to batch 
- sendkeys = done
- table filling
	batch creation
		check noteindex from VAT module
	invoice creation
		check form procedure

posting from batch routine = done 