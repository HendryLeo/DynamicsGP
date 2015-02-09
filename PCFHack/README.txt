this project is a hack around limitation of third party customization
we have Purchase Requisition module from our consultant that only allow 1 user to create price comparison and process to PO
but we have 2 user in our purchasing department that need to be able to do price comparison
after fiddling around the database, i conclude that a field hold the value that is being filtered by dexterity code to determined user_to_create_po

this hack allow user to change the value of that field to be his/her userid, so user can proceed to create price comparison and PO