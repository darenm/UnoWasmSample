# Getting Started with C and Uno

This document will walk through the steps to setup a basic compilation environment on Windows using Windows Subsystem for Linux (WSL).

> **INFO**: Learn more about WSL here: [What is the Windows Subsystem for Linux?](https://learn.microsoft.com/windows/wsl/about)

## Configuring Your Environment

1. Install WSL and Ubuntu following the instructions here:

   * [Install Ubuntu on WSL2 on Windows 11 with GUI support](https://ubuntu.com/tutorials/install-ubuntu-on-wsl2-on-windows-11-with-gui-support#1-overview)

    > **NOTE**: GUI support isn't necessary for this guide, but it's nice to have anyway.

1. Next, we need to install some useful or required packages. In your Ubuntu terminal, execute the following commands:

    ```bash
    sudo apt install unzip build-essential cmake apt-file pkg-config
    sudo apt install mesa-common-dev libglu1-mesa-dev libxi-dev
    sudo apt install libxrandr-dev
    ```

## Installing Emscripten

Emscripten is used to replace the C compiler and linker with a toolset that generates WebAssembly (WASM) instead.

1. First, navigate to your repository folder.

1. Get the emsdk repo

    ```bash
    git clone https://github.com/emscripten-core/emsdk.git
    ```

1. Enter that directory

    ```bash
    cd emsdk
    ```

1. Download and install the latest SDK tools

    ```bash
    ./emsdk install latest
    ```

1. Make the "latest" SDK "active"

    ```bash
    ./emsdk activate latest
    ```

1. Activate PATH and other environment variables

    ```bash
    source ./emsdk_env.sh
    ```

1. The current version of emscripten used by Uno is 3.1.12, so to install and activate that:

    ```bash
    ./emsdk install 3.1.12
    ./emsdk activate 3.1.12
    source ./emsdk_env.sh
    ```

    Test the installation by running `emcc --version`:

    ```bash
    emcc (Emscripten gcc/clang-like replacement + linker emulating GNU ld) 3.1.12 (38d1292ba2f5b4a7c8518931f5ae6f97ef0f6827)
    Copyright (C) 2014 the Emscripten authors (see AUTHORS.txt)
    This is free and open source software under the MIT license.
    There is NO warranty; not even for MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.
    ```

1. Finally, many scripts assume emscripten is installed in `/opt/emsdk/`, so create a symbolic link from that location to where you cloned emscripten:

    ```bash
    sudo ln -s <path-to-emsdk> /opt/emsdk
    ```

## Using `apt-get` to Find a Missing Package

When compiling C/C++ programs, it can be common to have missing dependencies reported during compilation or linking:

```c
In file included from thirdparty/freeglut/include/GL/freeglut.h:17:0,
                 from platform/gl/gl-app.h:36,
                 from platform/gl/gl-annotate.c:23:
thirdparty/freeglut/include/GL/freeglut_std.h:143:13: fatal error: GL/gl.h: No such file or directory
 #   include <GL/gl.h>
             ^~~~~~~~~
```

1. To determine what package must be installed to satisfy the requirement, enter the following commands:

    ```bash
    # to install the `apt-file` package
    sudo apt install apt-file
    # to preload the package cache
    sudo apt-file update
    # to search for the missing file
    sudo apt-file search "GL/gl.h"
    ```

    This will output many results similar to:

    ```bash
    emscripten: /usr/share/emscripten/system/include/GL/gl.h
    libogre-1.9-dev: /usr/include/OGRE/RenderSystems/GL/GL/gl.h
    mesa-common-dev: /usr/include/GL/gl.h
    mingw-w64-common: /usr/share/mingw-w64/include/GL/gl.h
    mingw-w64-i686-dev: /usr/i686-w64-mingw32/include/GL/gl.h
    mingw-w64-x86-64-dev: /usr/x86_64-w64-mingw32/include/GL/gl.h
    nvidia-340-dev: /usr/include/nvidia-340/GL/gl.h
    ```

    The best result is the **mesa-common-dev** package.

1. To install the **mesa-common-dev** package, run:

    ```bash
    sudo apt install mesa-common-dev
    ```

1. For the error:

    ```c
        CC build/release/platform/gl/gl-annotate.o
    In file included from thirdparty/freeglut/include/GL/freeglut.h:17:0,
                     from platform/gl/gl-app.h:36,
                     from platform/gl/gl-annotate.c:23:
    thirdparty/freeglut/include/GL/freeglut_std.h:144:13: fatal error: GL/glu.h: No such file or directory
     #   include <GL/glu.h>
                 ^~~~~~~~~~
    ```

    Enter the following command to find the package:

    ```bash
    sudo apt-file search "GL/glu.h"
    ```

1. To install the package, enter:

    ```bash
    sudo apt install libglu1-mesa-dev
    ```

## Create a C API

<https://stunlock.gg/posts/emscripten_with_cmake/>
<https://www.codeintrinsic.com/getting-started-with-cmake-for-c-cpp>

In this section we will create a simple C API that generates a Mandelbrot set and renders it to a JPEG file.

1. Navigate to your local repository folder and create a new folder named **MandelbrotAPI** and create the following folder hierarchy:

    * build
    * source
      * test
      * lib
    * thirdparty

2. Navigate to the **source/lib** folder and create **mandelbrotlib.h** as:

```c
// mandelbrotlib.h

```
