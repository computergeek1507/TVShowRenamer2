#-------------------------------------------------
#
# Project created by QtCreator 2012-11-03T21:57:31
#
#-------------------------------------------------

QT       += core gui network

TARGET = Renamer
TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    tvshowmodel.cpp \
    tvshowclass.cpp \
    convertionsettingsclass.cpp

HEADERS  += mainwindow.h \
    tvshowmodel.h \
    tvshowclass.h \
    convertionsettingsclass.h

FORMS    += mainwindow.ui \
    ConvertionSettings.ui

RESOURCES += \
    resources.qrc
