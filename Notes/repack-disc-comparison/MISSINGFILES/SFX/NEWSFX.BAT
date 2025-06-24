@echo off
echo Copying over new sounds...
copy grenln08.raw 0401gren.raw 
copy prfl2b_1.raw 0302puls.raw 
copy flamloop.raw 0503-fla.raw 
copy newhand4.raw 0602hand.raw 
copy newshot2.raw 0702shot.raw 
copy shipft1.raw 0208ripl.raw 
copy smart1.raw 0902smar.raw
copy 3101op1.raw 3101open.raw
copy 0202ripq.raw 0202ripl.raw
copy 1604skiq.raw 1604skit.raw
copy 0204ripq.raw 0204ripl.raw
copy 0206ripq.raw 0206ripl.raw
copy shipft8.raw 0208ripl.raw
copy enemyprf.raw enemyprf.raw 
copy smart3a2.raw smart3a2.raw 


echo Updating T:

md t:\trilogy\11_xfer\sfx
copy grenln08.raw t:\trilogy\11_xfer\sfx\0401gren.raw 
copy prfl2b_1.raw t:\trilogy\11_xfer\sfx\0302puls.raw 
copy flamloop.raw t:\trilogy\11_xfer\sfx\0503-fla.raw 
copy newhand4.raw t:\trilogy\11_xfer\sfx\0602hand.raw 
copy newshot2.raw t:\trilogy\11_xfer\sfx\0702shot.raw 
copy shipft1.raw t:\trilogy\11_xfer\sfx\0208ripl.raw 
copy smart1.raw t:\trilogy\11_xfer\sfx\0902smar.raw
copy 3101op1.raw t:\trilogy\11_xfer\sfx\3101open.raw
copy enemyprf.raw t:\trilogy\11_xfer\sfx\enemyprf.raw
copy smart3a2.raw t:\trilogy\11_xfer\sfx\smart3a2.raw
copy 0202ripq.raw t:\trilogy\11_xfer\sfx\0202ripl.raw
copy 1604skiq.raw t:\trilogy\11_xfer\sfx\1604skit.raw
copy 0204ripq.raw t:\trilogy\11_xfer\sfx\0204ripl.raw
copy 0206ripq.raw t:\trilogy\11_xfer\sfx\0206ripl.raw
copy shipft8.raw t:\trilogy\11_xfer\sfx\0208ripl.raw

copy ???sfx.txt t:\trilogy\11_xfer\sfx

