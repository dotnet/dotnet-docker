import sys
import this
import subprocess
import os
if sys.version_info[0] < 3:
    raise Exception("Must be using Python 3")
else:
    s = "".join([this.d.get(c, c) for c in this.s])
    print("*" * 5,"Zen of Alpine Python 3","*" * 5,"\n")
    print(s)
    print(subprocess.call(["dotnet", "dotnetapp.dll"]))