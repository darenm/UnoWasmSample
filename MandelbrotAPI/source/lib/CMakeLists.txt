if (DEFINED EMSCRIPTEN)
	add_executable(MandelbrotLib mandelbrotlib.c mandelbrotlib.h)

	set(CMAKE_EXECUTABLE_SUFFIX ".bc")

	set_target_properties(MandelbrotLib PROPERTIES COMPILE_FLAGS "-Os -s SIDE_MODULE=1 -r")
	set_target_properties(MandelbrotLib PROPERTIES LINK_FLAGS    "-Os -s WASM=1 -r -s SIDE_MODULE=1 -s STANDALONE_WASM --no-entry")
else()
	add_library(MandelbrotLib STATIC mandelbrotlib.c)
endif()