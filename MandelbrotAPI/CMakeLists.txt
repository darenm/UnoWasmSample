cmake_minimum_required(VERSION 3.8)

project(mandelbrot VERSION 1.0)

set( CMAKE_RUNTIME_OUTPUT_DIRECTORY "${PROJECT_SOURCE_DIR}/release")
# set( CMAKE_RUNTIME_OUTPUT_DIRECTORY "${PROJECT_BINARY_DIR}")
set( CMAKE_ARCHIVE_OUTPUT_DIRECTORY "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
set( CMAKE_LIBRARY_OUTPUT_DIRECTORY "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")
set( CMAKE_RUNTIME_OUTPUT_DIRECTORY "${CMAKE_RUNTIME_OUTPUT_DIRECTORY}")

# This causes the CMakeLists.txt in the sub directories to be evaluated
add_subdirectory(source/lib)

if (NOT DEFINED EMSCRIPTEN)
	add_subdirectory(source/test)
endif()
