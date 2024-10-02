#!/bin/bash
mkdir ./bin
mcs -r:System.Drawing.dll -r:System.Windows.Forms.dll ./src/*.cs -out:./bin/ElectricFieldVis.exe