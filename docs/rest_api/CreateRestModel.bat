@echo off

DEL RestMessages.cs

set DevEnvDir="C:\Program Files (x86)\Microsoft Visual Studio 14.0\Common7\Tools"
call %DevEnvDir%\..\Tools\vsvars32.bat

REM REM REM KILL KILL KILL

@xsd.exe /c /l:C# /n:Sportradar.MTS.SDK.Entities.Internal.REST UnifiedFeedDescriptions.xsd

rename UnifiedFeedDescriptions.cs RestMessages.cs