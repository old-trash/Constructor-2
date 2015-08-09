call ../../УказатьПуть.bat
del Клиент.exe
csc /target:winexe /out:Клиент.exe ..\НаборТаймеров\*.cs ..\Общее\*.cs ..\Соединение\SharpZipLib\*.cs ..\Соединение\*.cs ..\ЭлементВвода\*.cs ..\ЭлементВывода\*.cs ..\*.cs *.cs>Журнал.txt
pause
