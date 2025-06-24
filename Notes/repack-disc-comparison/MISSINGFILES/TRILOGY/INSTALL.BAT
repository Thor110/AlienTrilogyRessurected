@echo off
inst.exe
if ERRORLEVEL 1 GOTO ERROR
goto all_ok
:error
cls
echo Installation aborted!!!
echo -----------------------
echo Alien Trilogy may *not* be installed correctly.
echo Please run INSTALL again.
goto end
:all_ok
echo Alien Trilogy.
echo Installation completed without errors.
echo (Windows 95 users may have to close this window.)

:end

