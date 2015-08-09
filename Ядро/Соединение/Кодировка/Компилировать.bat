call ../../../УказатьПуть.bat
del Кодировка.exe
csc /target:exe /out:Кодировка.exe *.cs>Журнал.txt
pause
