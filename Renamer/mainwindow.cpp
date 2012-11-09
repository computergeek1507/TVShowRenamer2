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
    QStringList files = QFileDialog::getOpenFileNames(
                             this,
                             "Select Media File to Add",
                             QDir::homePath(),
                             "Videos (*.avi *.mp4 *.mkv)");
    Q_FOREACH(QString filename,files)
    {
        QDir fullinfo(filename);
        QFileInfo fi(filename);
        _TVShowModelList->addTVShowItem(TVShowClass(fullinfo.toNativeSeparators(fi.absoluteDir().path()), fi.fileName(),fi.completeSuffix()));
    }
}

void MainWindow::on_actionAdd_Folder_triggered()
{
    QString dir = QFileDialog::getExistingDirectory(this,"Open Directory",
                                                 QDir::homePath(),
                                                 QFileDialog::ShowDirsOnly
                                                 | QFileDialog::DontResolveSymlinks);
}

void MainWindow::on_actionRemove_Selected_triggered()
{
    int index = ui->tableViewTVShowList->currentIndex().row();
    _TVShowModelList->removeSingleRow(index);
}

void MainWindow::on_actionClear_List_triggered()
{
    _TVShowModelList->removeAll();
}

void MainWindow::on_actionExit_triggered()
{
    exit(0);
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

bool MainWindow::ConvertFileName()
{

    return true;
}
