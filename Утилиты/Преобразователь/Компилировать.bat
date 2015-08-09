call ../../УказатьПуть.bat
del Преобразователь.exe
csc /target:exe /out:Преобразователь.exe *.cs>Журнал.txt
pause
