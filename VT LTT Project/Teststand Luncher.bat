@echo off
REM Path to  file LTT Teststand Environment
SET testEnvironment = "D:\eDCU LTT\VT LTT Project\NI Teststand\LTT Teststand Environment 64bit.tsenv"

cd "C:\Program Files\National Instruments\TestStand 2023\Bin"

start SeqEdit.exe /env "D:\eDCU LTT\VT LTT Project\NI Teststand\LTT Teststand Environment 64bit.tsenv" && exit