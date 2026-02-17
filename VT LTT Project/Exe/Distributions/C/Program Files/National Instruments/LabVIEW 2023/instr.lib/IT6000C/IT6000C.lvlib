<?xml version='1.0' encoding='UTF-8'?>
<Library LVVersion="23008000">
	<Property Name="Instrument Driver" Type="Str">True</Property>
	<Property Name="NI.Lib.DefaultMenu" Type="Str">dir.mnu</Property>
	<Property Name="NI.Lib.Description" Type="Str">LabVIEW Plug and Play instrument driver for IT6000C.</Property>
	<Property Name="NI.Lib.Icon" Type="Bin">)Q#!!!!!!!)!"1!&amp;!!!-!%!!!@````]!!!!"!!%!!!)S!!!*Q(C=\&gt;3R&lt;2N"%)8B*]/"5X:AM)68A1'[B'NB=A@'J1I:+T'G"&lt;&lt;!&amp;K9&amp;NM"1[@G`Z=C199*-,-!'&gt;-OBS,&gt;\OR`X4C?.YZ0U5=O^Y`HGM@9PT_OQ;`83X[^VZ+PW`(P`MNTO`[.&gt;/@`K_L@^OHG]^\`X`^`^&gt;Q\'0.Y&gt;^-;(^$#E6&amp;&amp;*"?8R[Z9C,`)C,`)C,`)E4`)E4`)E4`)A$`)A$`)A$X+4G^TE*D?ZS5=D&amp;\H)2=[O&amp;)M8#R74&amp;B-5A['I?#M]B;@Q&amp;"[_KP!5HM*4?!I01V2Y#E`B+4S&amp;B^.5?!J0Y3E]B9?JBK2')]&gt;4?*B?C3@R**\%EXB95IEH!33,*2-HE]"1UJF]3$S**`(Q59EH]33?R*.Y[&amp;&lt;C34S**`%E(EY:OZ*$MT:S0%SDQ".Y!E`A#4R-L=!4?!*0Y!E],+@!%XA#2,"A-$E%"3=&amp;!Y)PA3@Q]%?"*`!%HM!4?/A;6SD'TKS;N:(D-2\D-2\D-2[GE0%9D`%9D`%QL9T(?)T(?)S(J71]RG-]"G)7:8G:S=S*:J!*D)@8O&amp;M]LF)/C5@L@]X,D;K_!&gt;5XFPK'5&gt;])[AOMPH$K#[,?;05'KD&gt;'`9060U1.6#_MHF!^5'@?4^32/F"\;K9G;E&gt;NK=V[[F]??$[@&gt;4K&gt;&gt;$Q?&gt;4A=N.`P.=_TJGH3&lt;L@4&gt;LP6:L0Z^&lt;4[4,M=$S`0J5=_`ZDH&lt;U^@JS^0X[G*YPO;PW6&gt;HEP`QL.2(\3]?MST2T]"_Y&lt;*F1!!!!!</Property>
	<Property Name="NI.Lib.SourceVersion" Type="Int">587235328</Property>
	<Property Name="NI.Lib.Version" Type="Str">1.0.0.0</Property>
	<Property Name="NI.LV.All.SourceOnly" Type="Bool">false</Property>
	<Item Name="Private" Type="Folder">
		<Property Name="NI.LibItem.Scope" Type="Int">2</Property>
		<Property Name="NI.SortType" Type="Int">3</Property>
		<Item Name="Default Instrument Setup.vi" Type="VI" URL="../Private/Default Instrument Setup.vi"/>
	</Item>
	<Item Name="Public" Type="Folder">
		<Property Name="NI.LibItem.Scope" Type="Int">1</Property>
		<Property Name="NI.SortType" Type="Int">3</Property>
		<Item Name="Action-Status" Type="Folder"/>
		<Item Name="Configure" Type="Folder">
			<Item Name="Abort" Type="Folder"/>
			<Item Name="Initiate" Type="Folder"/>
			<Item Name="Status" Type="Folder"/>
			<Item Name="Parallel" Type="Folder"/>
			<Item Name="Sense" Type="Folder"/>
			<Item Name="IO" Type="Folder"/>
			<Item Name="Format" Type="Folder"/>
			<Item Name="Battery" Type="Folder"/>
			<Item Name="Output" Type="Folder">
				<Item Name="Enable Source Output State.vi" Type="VI" URL="../Public/Configure/Output/Enable Source Output State.vi"/>
			</Item>
			<Item Name="Solar" Type="Folder"/>
			<Item Name="Trigger" Type="Folder"/>
			<Item Name="Source" Type="Folder">
				<Item Name="Configure Source Function Type.vi" Type="VI" URL="../Public/Configure/Source/Configure Source Function Type.vi"/>
				<Item Name="Configure Source Current Level.vi" Type="VI" URL="../Public/Configure/Source/Configure Source Current Level.vi"/>
				<Item Name="Enable Source OCP State.vi" Type="VI" URL="../Public/Configure/Source/Enable Source OCP State.vi"/>
				<Item Name="Configure Source OCP Level.vi" Type="VI" URL="../Public/Configure/Source/Configure Source OCP Level.vi"/>
				<Item Name="Configure Source Current Limit.vi" Type="VI" URL="../Public/Configure/Source/Configure Source Current Limit.vi"/>
				<Item Name="Enable Source OVP State.vi" Type="VI" URL="../Public/Configure/Source/Enable Source OVP State.vi"/>
				<Item Name="Configure Source OVP Level.vi" Type="VI" URL="../Public/Configure/Source/Configure Source OVP Level.vi"/>
				<Item Name="Configure Source Voltage Level.vi" Type="VI" URL="../Public/Configure/Source/Configure Source Voltage Level.vi"/>
				<Item Name="Configure Source Voltage Limit.vi" Type="VI" URL="../Public/Configure/Source/Configure Source Voltage Limit.vi"/>
			</Item>
			<Item Name="Battery Emulator" Type="Folder"/>
			<Item Name="ARB" Type="Folder"/>
		</Item>
		<Item Name="Data" Type="Folder">
			<Item Name="Read Output.vi" Type="VI" URL="../Public/Data/Read Output.vi"/>
		</Item>
		<Item Name="Utility" Type="Folder">
			<Item Name="Error Query.vi" Type="VI" URL="../Public/Utility/Error Query.vi"/>
			<Item Name="Reset.vi" Type="VI" URL="../Public/Utility/Reset.vi"/>
			<Item Name="Self-Test.vi" Type="VI" URL="../Public/Utility/Self-Test.vi"/>
			<Item Name="Remote.vi" Type="VI" URL="../Public/Utility/Remote.vi"/>
		</Item>
		<Item Name="Close.vi" Type="VI" URL="../Public/Close.vi"/>
		<Item Name="Initialize.vi" Type="VI" URL="../Public/Initialize.vi"/>
	</Item>
</Library>
