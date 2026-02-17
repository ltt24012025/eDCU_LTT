** Simple TestStand Operator Interface Example **

This workspace contains a simple example of a TestStand operator 
interface implemented using MFC.  This is a dialog based application.

This example generates the header and implementation files for accessing the TestStand API and
the API's for the TestStand ActiveX User Interface controls with the #import compiler directive.  
#Import is also known as Compiler COM support. 

The ActiveX api's throw _com_error exceptions, which the example handles appropriately.

