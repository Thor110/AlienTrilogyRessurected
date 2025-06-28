@echo off
echo Directory cleanup script.
echo Please be sure to run this from the main game folder. IE : "Alien Trilogy\HDD\TRILOGY" for the repack.
echo This script will delete all unused files and replace known unused, but required files with empty files.
set /p confirm=Do you want to delete all unused files? (Y/N)? 
if /i not "%confirm%"=="Y" exit
pause
for /f "delims=" %%i in (delete.txt) do (
	if exist "%%i" (
	  del %%i
	)
)
for /f "delims=" %%i in (create.txt) do (
	if exist "%%i" (
	  type nul > %%i
	)
)
del delete.txt
del create.txt
echo Cleanup complete. Deleting this script...
:: Self-delete at end of script
cmd /c "timeout 2 >nul & del \"%~f0\""