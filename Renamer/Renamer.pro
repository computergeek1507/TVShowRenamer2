
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
    tvepisodetitle.cpp \
    settingsdialog.cpp \
    convertiondialog.cpp \
    foldersettingsdialog.cpp

HEADERS  += mainwindow.h \
    tvshowmodel.h \
    tvshowclass.h \
    tvepisodetitle.h \
    settingsdialog.h \
    convertiondialog.h \
    foldersettingsdialog.h

FORMS    += mainwindow.ui \
    settingsdialog.ui \
    convertiondialog.ui \
    foldersettingsdialog.ui

RESOURCES += \
    resources.qrc

