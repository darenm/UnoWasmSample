# Getting Started with C and Uno

This document will walk through the steps to setup a basic compilation environment on Windows using Windows Subsystem for Linux (WSL).

> **INFO**: Learn more about WSL here: [What is the Windows Subsystem for Linux?](https://learn.microsoft.com/windows/wsl/about)

## Configuring Your Environment

1. Install WSL and Ubuntu following the instructions here:

   * [Install Ubuntu on WSL2 on Windows 11 with GUI support](https://ubuntu.com/tutorials/install-ubuntu-on-wsl2-on-windows-11-with-gui-support#1-overview)

    > **NOTE**: GUI support isn't necessary for this guide, but it's nice to have anyway.

1. Next, we need to install some useful or required packages. In your Ubuntu terminal, execute the following commands:

    ```bash
    sudo apt install unzip # makes it simple to unzip .zip files
    sudo apt install build-essential # compilers
    sudo apt install cmake # cmake build system
    ```

## Create a C API

In this section we will create a simple C API that generates a Mandelbrot set and renders it to a JPEG file.

1. Navigate to your local repository folder and create a new folder named **MandelbrotAPI**.