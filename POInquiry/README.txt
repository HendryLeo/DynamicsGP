This Project add an additional menu to open PO Inquiry from PO Entry Window
Some background info for this project
1. We have an alternate window of PO Entry
2. If we print from PO Entry, then the PO will be automatically set to be Released
3. If we print from PO Inquiry, then the PO status will not change

To achieve this customization, I use Continuum API. This technique is not supported by Microsoft.
To hunt down the sanscript I use script profiler from Dynamics GP
this is the script of interest
'OpenWindow() of form POP_Inquiry_PO_Entry', 0, "PO1400200", 2, 2
which translate to sanscript as
OpenWindow("PO1400200",2,2) of form POP_Inquiry_PO_Entry;
I am not sure what is the parameter list of OpenWindow but changing the string literal will achieve what I am looking for

as usual I am using alternate window, so I am using custom assembly
I tried to give the equivalent for original Dynamics window in the code, but it is not tested