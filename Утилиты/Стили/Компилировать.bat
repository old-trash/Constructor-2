call ../../УказатьПуть.bat
del Стили.exe
csc /target:exe /out:Стили.exe *.cs>Журнал.txt
pause

