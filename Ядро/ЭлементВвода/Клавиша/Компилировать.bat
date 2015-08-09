call ../../../УказатьПуть.bat
del Клавиша.exe
csc /target:winexe /out:Клавиша.exe *.cs>Журнал.txt
pause
