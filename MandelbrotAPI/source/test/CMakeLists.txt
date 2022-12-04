list(APPEND EXTRA_LIBS MandelbrotLib)
list(APPEND EXTRA_INCLUDES "${PROJECT_SOURCE_DIR}/source/lib")

add_executable(test test.c)

target_include_directories(test PUBLIC
                           "${PROJECT_BINARY_DIR}"
                           ${EXTRA_INCLUDES}
                           )

target_link_libraries(test PUBLIC MandelbrotLib)