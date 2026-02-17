<?xml version='1.0' encoding='UTF-8'?>
<Library LVVersion="23008000">
	<Property Name="Instrument Driver" Type="Str">True</Property>
	<Property Name="NI.Lib.DefaultMenu" Type="Str">dir.mnu</Property>
	<Property Name="NI.Lib.Description" Type="Str">LabVIEW Plug and Play instrument driver for IT-M3900D.</Property>
	<Property Name="NI.Lib.Icon" Type="Bin">)Q#!!!!!!!)!"1!&amp;!!!-!%!!!@````]!!!!"!!%!!!)`!!!*Q(C=\&gt;4.&lt;?*1&amp;)&lt;B,[-MMK7$%=P:HB;I)")JY&lt;4!.EN;/#W9%KA!T7H"$9Q5,\VV8F]/%[*%92/E'9FL,O$P`DV='UON0%DXGC[6]=MS&gt;[D;XM_$U_H98N0&lt;R^F2\&gt;0J^$DO1`P&lt;_0(4]?0\]?@L8`,LSX*LP\8`X_U8#HW?,X;[=J(OGJ3;V+![V&gt;KPGZ)]S:-]S:-]S9-]S)-]S)-]S*X=S:X=S:X=S9X=S)X=S)X=S.N",H+2CZR&gt;32:0&amp;EIG43:)/E.2]J:Y%E`C34S=+P%EHM34?")08:2Y%E`C34S*BW&amp;+0)EH]33?R-.54:,N)-?4?*B?A3@Q"*\!%XB95I%H!!3,"2-(E]"1U"B]#4S"*`$Q69%H]!3?Q".Y;&amp;&lt;A#4S"*`!%(I;U89GGG1^S0%QDR_.Y()`D=4R-,=@D?"S0YX%],#@(YXA=B,/A-TE%/9/=$M[*YX%]@-DR/"\(YXA=$UXN#HH&lt;G6ET(_2Y$)`B-4S'R`!QB1S0Y4%]BM@Q-+U-D_%R0)&lt;(],#5$)`B-4Q'R&amp;C5Z76-:AQU/BG"Y?(6\B:L6SG;R.J2@]XDD;K[!65XFOK'5&gt;U)KAOMOH#K#[,;;.5'KD:'^9.60U1&amp;6#WMGF$6510P0860\;B&lt;[I;[JK[I3_JC(PL.(9&gt;B5.`XWO`X[LJ/W_V7G]V'[`6;K^6+S_63C]8C\^0K*]?RX*W?3]^]\]@BZ@$9\8Y``&gt;E&gt;(H`N$E`$SZR@MR[@3``#MV%`.*U^ZNGD6UM0TWQ!!!!!</Property>
	<Property Name="NI.Lib.SourceVersion" Type="Int">587235328</Property>
	<Property Name="NI.Lib.Version" Type="Str">1.0.1.0</Property>
	<Property Name="NI.LV.All.SourceOnly" Type="Bool">false</Property>
	<Item Name="Private" Type="Folder">
		<Property Name="NI.LibItem.Scope" Type="Int">2</Property>
		<Property Name="NI.SortType" Type="Int">3</Property>
		<Item Name="Default Instrument Setup.vi" Type="VI" URL="../Private/Default Instrument Setup.vi"/>
	</Item>
	<Item Name="Public" Type="Folder">
		<Property Name="NI.LibItem.Scope" Type="Int">1</Property>
		<Property Name="NI.SortType" Type="Int">3</Property>
		<Item Name="ARB" Type="Folder"/>
		<Item Name="Battery" Type="Folder"/>
		<Item Name="Configure" Type="Folder">
			<Item Name="Abort" Type="Folder"/>
			<Item Name="Initiate" Type="Folder"/>
			<Item Name="IO" Type="Folder"/>
			<Item Name="Output" Type="Folder">
				<Item Name="Enable Source Output State.vi" Type="VI" URL="../Public/Configure/Output/Enable Source Output State.vi"/>
			</Item>
			<Item Name="Parallel" Type="Folder"/>
			<Item Name="Sense" Type="Folder"/>
			<Item Name="Source" Type="Folder">
				<Item Name="Configure Function Type.vi" Type="VI" URL="../Public/Configure/Source/Configure Function Type.vi"/>
				<Item Name="Configure Current Level.vi" Type="VI" URL="../Public/Configure/Source/Configure Current Level.vi"/>
				<Item Name="Configure Current Limit Positive.vi" Type="VI" URL="../Public/Configure/Source/Configure Current Limit Positive.vi"/>
				<Item Name="Configure OCP Delay.vi" Type="VI" URL="../Public/Configure/Source/Configure OCP Delay.vi"/>
				<Item Name="Configure OCP Level.vi" Type="VI" URL="../Public/Configure/Source/Configure OCP Level.vi"/>
				<Item Name="Configure OVP Delay.vi" Type="VI" URL="../Public/Configure/Source/Configure OVP Delay.vi"/>
				<Item Name="Configure OVP Level.vi" Type="VI" URL="../Public/Configure/Source/Configure OVP Level.vi"/>
				<Item Name="Configure Voltage Level.vi" Type="VI" URL="../Public/Configure/Source/Configure Voltage Level.vi"/>
				<Item Name="Configure Voltage Limit Positive.vi" Type="VI" URL="../Public/Configure/Source/Configure Voltage Limit Positive.vi"/>
				<Item Name="Enable OCP State.vi" Type="VI" URL="../Public/Configure/Source/Enable OCP State.vi"/>
				<Item Name="Enable OVP State.vi" Type="VI" URL="../Public/Configure/Source/Enable OVP State.vi"/>
			</Item>
			<Item Name="Status" Type="Folder"/>
			<Item Name="Trigger" Type="Folder"/>
		</Item>
		<Item Name="Data" Type="Folder">
			<Item Name="Read Output.vi" Type="VI" URL="../Public/Data/Read Output.vi"/>
		</Item>
		<Item Name="Utility" Type="Folder">
			<Item Name="Error Query.vi" Type="VI" URL="../Public/Utility/Error Query.vi"/>
			<Item Name="Remote.vi" Type="VI" URL="../Public/Utility/Remote.vi"/>
			<Item Name="Reset.vi" Type="VI" URL="../Public/Utility/Reset.vi"/>
			<Item Name="Self-Test.vi" Type="VI" URL="../Public/Utility/Self-Test.vi"/>
		</Item>
		<Item Name="Close.vi" Type="VI" URL="../Public/Close.vi"/>
		<Item Name="Initialize.vi" Type="VI" URL="../Public/Initialize.vi"/>
	</Item>
</Library>
