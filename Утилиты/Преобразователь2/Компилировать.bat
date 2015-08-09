call ../../УказатьПуть.bat
del Преобразователь2.exe
csc /target:winexe /out:Преобразователь2.exe *.cs>Журнал.txt
pause
