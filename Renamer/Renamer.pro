
#-------------------------------------------------
#
# Project created by QtCreator 2012-11-03T21:57:31
#
#-------------------------------------------------

QT       += core gui network

TARGET = Renamer

unix:LIBS += -L/usr/lib -lboost_regex
win32:LIBS +=C:/Qt/2010.02.1/qt/lib/libboost_regex.lib

TEMPLATE = app


SOURCES += main.cpp\
        mainwindow.cpp \
    tvshowmodel.cpp \
    tvshowclass.cpp

HEADERS  += mainwindow.h \
    tvshowmodel.h \
    tvshowclass.h

FORMS    += mainwindow.ui

RESOURCES += \
    resources.qrc

