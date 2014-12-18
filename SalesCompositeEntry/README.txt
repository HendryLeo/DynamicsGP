This Project use 3rd party Form and modified to add a local Currency Field
The Idea is to display additional information that already in the form but not visible
In this case the form already has field called ExchangeRate
I add local Currency Field (called XchRate) and register a trigger to ExchangeRate to update the XchRate whenever there is changes

I need to generate the Assembly for the 3rd party main form and also the modified form
dag <id> <dynamic.set> /M
dag <id> <dynamic.set> /F
and include the reference in VS Project