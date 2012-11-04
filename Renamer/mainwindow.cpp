#include "mainwindow.h"
#include "ui_mainwindow.h"

MainWindow::MainWindow(QWidget *parent) :
    QMainWindow(parent),
    ui(new Ui::MainWindow)
{
	//ui->tableViewTVShowList->setModel(new TVShowModel(ui->tableViewTVShowList));
    ui->setupUi(this);
    _TVShowModelList =  new TVShowModel(this);
    proxyModel = new QSortFilterProxyModel(this);
    proxyModel->setSourceModel(_TVShowModelList);
    proxyModel->setDynamicSortFilter(true);

    ui->tableViewTVShowList->setModel(proxyModel);
    ui->tableViewTVShowList->resizeColumnsToContents();
    //setStyleSheet("QMainWindow {background: 'light blue';}");
}

MainWindow::~MainWindow()
{
    delete ui;
}

void MainWindow::on_actionAdd_Files_triggered()
{

}

void MainWindow::on_actionAdd_Folder_triggered()
{

}

void MainWindow::on_actionRemove_Selected_triggered()
{

}

void MainWindow::on_actionClear_List_triggered()
{

}

void MainWindow::on_actionExit_triggered()
{

}

void MainWindow::on_pushButtonSave_clicked()
{

}

void MainWindow::on_pushButtonMove_clicked()
{

}

void MainWindow::on_pushButtonCopy_clicked()
{

}

void MainWindow::on_pushButtonGetTitle_clicked()
{

}
