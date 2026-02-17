TestStand no longer ships an executable version of the TestStand LabVIEW Runtime Server 
(TestStandLVRTS.exe).  The TestStand LabVIEW Runtime Server must be built into an 
executable before using it can be used in TestStand.

To create an executable for a Teststand LabVIEW Runtime Server using LabVIEW 8.0 and higher:

    Open TSLVRTS.lvproj and build the TestStandLVRTS build spec.

    Note: The default build script enables the ActiveX server for the resulting application
    with the ProgID prefix TestStandLVRTS.  


To use on a deployed system, distribute the following files:

    - TestStandLVRTS.exe

If TestStand launches the server to run LabVIEW VIs, it will automatically quit the server
on exit.  If you wish to manually quit a minimized server, you can either close the window
or press the STOP button on the server's panel.  If you wish to manually quit a hidden
server, you must kill the process TestStandLVRTS.exe.

Note:  National Instruments recommends that you use the LabVIEW Run-Time Engine Adapter
option for deployed systems.  The TestStand LabVIEW Runtime Server is provided for
compatability with older versions of TestStand.
