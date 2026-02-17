<?xml version='1.0' encoding='UTF-8'?>
<Project Type="Project" LVVersion="23008000">
	<Item Name="My Computer" Type="My Computer">
		<Property Name="CCSymbols" Type="Str">OS,Win;CPU,x86;</Property>
		<Property Name="NI.SortType" Type="Int">3</Property>
		<Property Name="server.app.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="server.control.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="server.tcp.enabled" Type="Bool">false</Property>
		<Property Name="server.tcp.port" Type="Int">0</Property>
		<Property Name="server.tcp.serviceName" Type="Str">My Computer/VI Server</Property>
		<Property Name="server.tcp.serviceName.default" Type="Str">My Computer/VI Server</Property>
		<Property Name="server.vi.callsEnabled" Type="Bool">true</Property>
		<Property Name="server.vi.propertiesEnabled" Type="Bool">true</Property>
		<Property Name="specify.custom.address" Type="Bool">false</Property>
		<Item Name="Top Level VIs" Type="Folder">
			<Item Name="TestStand - LabVIEW Runtime Server.vi" Type="VI" URL="../Server.llb/TestStand - LabVIEW Runtime Server.vi"/>
		</Item>
		<Item Name="Dependencies" Type="Dependencies">
			<Item Name="vi.lib" Type="Folder">
				<Item Name="Trim Whitespace.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Trim Whitespace.vi"/>
				<Item Name="whitespace.ctl" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/whitespace.ctl"/>
				<Item Name="NI_LVConfig.lvlib" Type="Library" URL="/&lt;vilib&gt;/Utility/config.llb/NI_LVConfig.lvlib"/>
				<Item Name="Clear Errors.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Clear Errors.vi"/>
				<Item Name="Check if File or Folder Exists.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/libraryn.llb/Check if File or Folder Exists.vi"/>
				<Item Name="NI_FileType.lvlib" Type="Library" URL="/&lt;vilib&gt;/Utility/lvfile.llb/NI_FileType.lvlib"/>
				<Item Name="Error Cluster From Error Code.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Error Cluster From Error Code.vi"/>
				<Item Name="NI_PackedLibraryUtility.lvlib" Type="Library" URL="/&lt;vilib&gt;/Utility/LVLibp/NI_PackedLibraryUtility.lvlib"/>
				<Item Name="8.6CompatibleGlobalVar.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/config.llb/8.6CompatibleGlobalVar.vi"/>
				<Item Name="Trim Whitespace One-Sided.vi" Type="VI" URL="/&lt;vilib&gt;/Utility/error.llb/Trim Whitespace One-Sided.vi"/>
				<Item Name="Space Constant.vi" Type="VI" URL="/&lt;vilib&gt;/dlg_ctls.llb/Space Constant.vi"/>
			</Item>
		</Item>
		<Item Name="Build Specifications" Type="Build">
			<Item Name="TestStandLVRTS" Type="EXE">
				<Property Name="App_INI_aliasGUID" Type="Str">{10C999D7-2C62-49DC-B2DC-EC688FCC13D4}</Property>
				<Property Name="App_INI_GUID" Type="Str">{25DBEB83-F05A-45CE-BC5A-0951F3F7BDC4}</Property>
				<Property Name="App_serverConfig.httpPort" Type="Int">8002</Property>
				<Property Name="App_serverType" Type="Int">1</Property>
				<Property Name="Bld_buildCacheID" Type="Str">{DA169CA9-8C1F-4752-BE02-816699E02316}</Property>
				<Property Name="Bld_buildSpecName" Type="Str">TestStandLVRTS</Property>
				<Property Name="Bld_excludeLibraryItems" Type="Bool">true</Property>
				<Property Name="Bld_excludePolymorphicVIs" Type="Bool">true</Property>
				<Property Name="Bld_excludeTypedefs" Type="Bool">true</Property>
				<Property Name="Bld_localDestDir" Type="Path">../Server</Property>
				<Property Name="Bld_localDestDirType" Type="Str">relativeToProject</Property>
				<Property Name="Bld_modifyLibraryFile" Type="Bool">true</Property>
				<Property Name="Bld_previewCacheID" Type="Str">{B80FF8CB-6139-4FC2-B4BA-75B598561608}</Property>
				<Property Name="Bld_targetDestDir" Type="Path"></Property>
				<Property Name="Bld_version.major" Type="Int">1</Property>
				<Property Name="Destination[0].destName" Type="Str">TestStandLVRTS.exe</Property>
				<Property Name="Destination[0].path" Type="Path">../LabVIEW/Server/TestStandLVRTS.exe</Property>
				<Property Name="Destination[0].type" Type="Str">App</Property>
				<Property Name="Destination[1].destName" Type="Str">data</Property>
				<Property Name="Destination[1].path" Type="Path">../LabVIEW/Server/data</Property>
				<Property Name="Destination[2].destName" Type="Str">Destination Directory</Property>
				<Property Name="Destination[2].path" Type="Path">../LabVIEW/Server</Property>
				<Property Name="DestinationCount" Type="Int">3</Property>
				<Property Name="Exe_actXinfo_enumCLSID[0]" Type="Str">{22534FCC-E3DB-46A5-82B0-9FF7E66A7DE0}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[1]" Type="Str">{6286ED40-0218-4763-AA9F-17FB9192FFF0}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[10]" Type="Str">{634C2348-9505-448D-B1B0-BC4F665B32E0}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[11]" Type="Str">{8FD8A487-81F1-450A-96B4-A08D7C80F1BE}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[12]" Type="Str">{895BC51C-C74B-410E-ADF9-756A718F7D6E}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[13]" Type="Str">{58E5CDDB-C220-4DCB-AF69-25AAF05AAD0A}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[14]" Type="Str">{1F91EDF6-2788-4262-BFFE-63AC981770FD}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[15]" Type="Str">{76706009-1D6F-4EAC-8108-FE4C590E24BB}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[16]" Type="Str">{5109A814-4635-4EAC-BB83-0101464F71CB}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[2]" Type="Str">{8AF49633-594F-4097-9465-99C8DA97FE28}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[3]" Type="Str">{8922C94D-685E-4F02-BADF-83665DFDAA5C}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[4]" Type="Str">{655DCA42-39B1-4C8E-86CF-ED01917ACBD4}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[5]" Type="Str">{F9080D10-E41E-4FA2-A96B-1D1D32967693}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[6]" Type="Str">{57A8B6FD-7BCA-462F-9BB8-5E30BF6011B5}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[7]" Type="Str">{E8036798-E3E9-4DDD-95E9-75ABCD3EFCAA}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[8]" Type="Str">{77A0AF3F-B402-479C-9E19-357D9BFC8E55}</Property>
				<Property Name="Exe_actXinfo_enumCLSID[9]" Type="Str">{3EC7EA84-B639-4DA6-B0FC-E22C1AC50C77}</Property>
				<Property Name="Exe_actXinfo_enumCLSIDsCount" Type="Int">17</Property>
				<Property Name="Exe_actXinfo_majorVersion" Type="Int">5</Property>
				<Property Name="Exe_actXinfo_minorVersion" Type="Int">5</Property>
				<Property Name="Exe_actXinfo_objCLSID[0]" Type="Str">{D4DD93BB-03B7-4C34-AC25-86E2C912FA7E}</Property>
				<Property Name="Exe_actXinfo_objCLSID[1]" Type="Str">{D8BC7B63-188C-4091-898F-5941E086FABF}</Property>
				<Property Name="Exe_actXinfo_objCLSID[2]" Type="Str">{ED15AA37-A03F-48CC-89C1-F3BD9153BF43}</Property>
				<Property Name="Exe_actXinfo_objCLSID[3]" Type="Str">{45A2468E-0A94-4B94-B956-9059E14E4F41}</Property>
				<Property Name="Exe_actXinfo_objCLSID[4]" Type="Str">{40111404-0994-4594-B8B5-3EE544599494}</Property>
				<Property Name="Exe_actXinfo_objCLSID[5]" Type="Str">{FF3E6707-CDF9-4B69-97AF-A5FE153A112A}</Property>
				<Property Name="Exe_actXinfo_objCLSIDsCount" Type="Int">6</Property>
				<Property Name="Exe_actXinfo_progIDPrefix" Type="Str">TestStandLVRTS</Property>
				<Property Name="Exe_actXServerName" Type="Str">TestStandLVRTS</Property>
				<Property Name="Exe_actXServerNameGUID" Type="Str"></Property>
				<Property Name="Source[0].itemID" Type="Str">{CBA4381F-9574-48F2-98D0-AB1BF51F02C1}</Property>
				<Property Name="Source[0].type" Type="Str">Container</Property>
				<Property Name="Source[1].destinationIndex" Type="Int">0</Property>
				<Property Name="Source[1].itemID" Type="Ref">/My Computer/Top Level VIs/TestStand - LabVIEW Runtime Server.vi</Property>
				<Property Name="Source[1].properties[0].type" Type="Str">Allow debugging</Property>
				<Property Name="Source[1].properties[0].value" Type="Bool">false</Property>
				<Property Name="Source[1].properties[1].type" Type="Str">Remove block diagram</Property>
				<Property Name="Source[1].properties[1].value" Type="Bool">true</Property>
				<Property Name="Source[1].properties[2].type" Type="Str">Run when opened</Property>
				<Property Name="Source[1].properties[2].value" Type="Bool">true</Property>
				<Property Name="Source[1].properties[3].type" Type="Str">Show Abort button</Property>
				<Property Name="Source[1].properties[3].value" Type="Bool">false</Property>
				<Property Name="Source[1].propertiesCount" Type="Int">4</Property>
				<Property Name="Source[1].sourceInclusion" Type="Str">TopLevel</Property>
				<Property Name="Source[1].type" Type="Str">VI</Property>
				<Property Name="SourceCount" Type="Int">2</Property>
				<Property Name="TgtF_companyName" Type="Str">Microsoft</Property>
				<Property Name="TgtF_fileDescription" Type="Str">TestStandLVRTS.exe</Property>
				<Property Name="TgtF_internalName" Type="Str">TestStandLVRTS.exe</Property>
				<Property Name="TgtF_productName" Type="Str">TestStandLVRTS.exe</Property>
				<Property Name="TgtF_targetfileGUID" Type="Str">{3D10A7C2-7D31-4B41-8885-F2CF6BF20E1B}</Property>
				<Property Name="TgtF_targetfileName" Type="Str">TestStandLVRTS.exe</Property>
			</Item>
		</Item>
	</Item>
</Project>
