#!/bin/bash

cd build
cmake ..
cd ../build.em
emcmake cmake ..
cd ..
