import ntplib
from time import ctime, time

addr_remote = '13.64.103.179'

c = ntplib.NTPClient()
remote = c.request(addr_remote)
local = time()
print("REMOTE: " + ctime(remote.tx_time) + "   <reference clock>   ") 
print("LOCAL:  " + ctime(local)        + "   delta: " + str(local - remote.tx_time ))