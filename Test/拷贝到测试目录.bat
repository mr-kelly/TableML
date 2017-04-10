:: copy files to debug dir

cd %~dp0

xcopy settingsrc\* ..\TableML\TableMLCompilerConsole\bin\Debug\settingsrc /S/Y/R/I

pause